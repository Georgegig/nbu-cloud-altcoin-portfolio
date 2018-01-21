using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using PortfolioCommon;
using PortfolioCommon.Access;
using PortfolioCommon.Entities;
using PortfolioCommon.Interfaces;

namespace WorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private CloudQueueAccess _cloudQueueAccess;
        private IPortfolioDataAccess _dataAccess;

        public override void Run()
        {
            Trace.WriteLine("Portfolio WorkerRole entry point called", "Information");

            while (true)
            {
                Thread.Sleep(15000);

                this.GetPortfolioMessage();
            }
        }

        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = 12;
            Trace.AutoFlush = true;

            _dataAccess = new SqlServerDataAccess();
            _cloudQueueAccess = new CloudQueueAccess();

            initializeStorage();

            return base.OnStart();
        }

        private void initializeStorage()
        {
            File.Delete(@"C:\PortfolioWorkerRole.log");

            //_dataAccess.ClearData();
            _cloudQueueAccess.ClearDrawRaffleQueue();
        }

        private void GetPortfolioMessage()
        {
            CloudQueueMessage getPortfolioMessage = _cloudQueueAccess.ReadGetPortfolioMessage();

            if (getPortfolioMessage != null)
            {
                string userEmail;

                try
                {
                    userEmail = getPortfolioMessage.AsString;
                    Trace.WriteLine("Processing Portfolio for user: " + userEmail.ToString());
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Cannot process email from message." + ex.Message);
                    _cloudQueueAccess.DeleteGetPortfolioMessage(getPortfolioMessage);

                    return;
                }

                try
                {
                    List<CoinEntity> portfolio = _dataAccess.GetUserPortfolio(userEmail);

                    if (portfolio == null || portfolio.Count == 0)
                    {
                        Trace.WriteLine("Portfolio not found or empty. User email: " + userEmail);

                        _cloudQueueAccess.DeleteGetPortfolioMessage(getPortfolioMessage);
                        
                    }
                    else
                    {
                        //Business logic
                        //Get coins from api. Refresh coins.
                        for (int i = 0; i < portfolio.Count; i++)
                        {
                            WebRequest request = WebRequest.Create(string.Format(Constants.GET_COIN_INFORMATION, portfolio[i].Id));
                            request.Credentials = CredentialCache.DefaultCredentials;
                            WebResponse response = request.GetResponse();
                            Trace.WriteLine(((HttpWebResponse)response).StatusDescription);
                            Stream dataStream = response.GetResponseStream();
                            StreamReader reader = new StreamReader(dataStream);
                            string responseFromServer = reader.ReadToEnd();
                            JavaScriptSerializer jser = new JavaScriptSerializer();
                            List<CoinEntity> currCoin = jser.Deserialize<List<CoinEntity>>(responseFromServer);
                            Trace.WriteLine(responseFromServer);
                            this._dataAccess.UpdateCoinPrice(userEmail, currCoin[0]);
                            reader.Close();
                            response.Close();
                        }                       

                        Trace.WriteLine("Portfolio refreshed. User email: " + userEmail);

                        _cloudQueueAccess.DeleteGetPortfolioMessage(getPortfolioMessage);
                    }                   
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Error processing raffle." + ex.Message);
                }
            }
        }
    }
}
