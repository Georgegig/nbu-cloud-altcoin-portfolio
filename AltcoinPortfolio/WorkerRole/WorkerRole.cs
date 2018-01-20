using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using PortfolioCommon.Access;
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

                //this.UpdatePortfolioMessage();
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
                Guid raffleId;

                try
                {
                    raffleId = Guid.Parse(getPortfolioMessage.AsString);
                    Trace.WriteLine("Processing Raffle id: " + raffleId.ToString());
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Cannot parse Id from message." + ex.Message);
                    _cloudQueueAccess.DeleteGetPortfolioMessage(getPortfolioMessage);

                    return;
                }

                try
                {
                    //RaffleEntity raffle = _dataAccess.ReadRaffle(raffleId);

                    //if (raffle == null)
                    //{
                    //    Trace.WriteLine("Raffle not found. Raffle ID: " + raffleId);

                    //    _cloudQueueAccess.DeleteDrawRaffleMessage(drawRaffleMessage);
                    //    continue;
                    //}

                    //if (raffle.GetStatus() == RaffleStatus.Processed)
                    //{
                    //    Trace.WriteLine("Raffle is already precessed. Raffle ID: " + raffleId);

                    //    _cloudQueueAccess.DeleteDrawRaffleMessage(drawRaffleMessage);
                    //    continue;
                    //}

                    //var random = new Random();
                    //raffle.WinningNumber = random.Next(1, 6);

                    //List<BetEntity> betsForRaffle = _dataAccess.ReadBetsForRaffle(raffleId);
                    //List<int> winningTicketNumbers = new List<int>();

                    //foreach (var bet in betsForRaffle)
                    //{
                    //    if (bet.BetNumber == raffle.WinningNumber)
                    //    {
                    //        winningTicketNumbers.Add(bet.TicketNumber);
                    //    }
                    //}

                    //raffle.SetStatus(RaffleStatus.Processed);
                    //raffle.UpdateDate = DateTime.Now;

                    //if (winningTicketNumbers.Count > 0)
                    //{
                    //    raffle.WinningTicketsResult = string.Join(", ", winningTicketNumbers);
                    //}
                    //else
                    //{
                    //    raffle.WinningTicketsResult = "No winning tickets.";
                    //}

                    //_dataAccess.UpdateRaffle(raffle);

                    //Trace.WriteLine("Raffle processed. Raffle ID: " + raffleId);
                    //Trace.WriteLine("Winning number: " + raffle.WinningNumber);

                    _cloudQueueAccess.DeleteGetPortfolioMessage(getPortfolioMessage);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Error processing raffle." + ex.Message);
                }
            }
        }
    }
}
