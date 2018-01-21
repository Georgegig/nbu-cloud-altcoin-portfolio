using PortfolioCommon.Access;
using PortfolioCommon.Entities;
using PortfolioCommon.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioCommon.Managers
{
    public class PortfolioManager
    {
        private static object _lock = new object();

        private IPortfolioDataAccess _portfolioDataAccess;
        private CloudQueueAccess _cloudQueueAccess;
        private CloudBlobAccess _cloudBlobAccess;

        public PortfolioManager(IPortfolioDataAccess UserdataAccess, CloudQueueAccess cloudQueueAccess, CloudBlobAccess cloudBlobAccess)
        {
            _portfolioDataAccess = UserdataAccess;
            _cloudQueueAccess = cloudQueueAccess;
            _cloudBlobAccess = cloudBlobAccess;
        }

        public UserEntity GetUser(Guid userId)
        {
            return _portfolioDataAccess.GetUser(userId);
        }

        public void RegisterUser(UserEntity user)
        {
            this._portfolioDataAccess.RegisterUser(user);
        }

        public string GetUserPassword(string email)
        {
            return _portfolioDataAccess.GetUserPassword(email);
        }

        public List<CoinEntity> GetUserPortfolio(string email)
        {
            List<CoinEntity> portfolio = this._portfolioDataAccess.GetUserPortfolio(email);
            this.uploadPortfolio(email, portfolio);
            return portfolio;
        }

        private void uploadPortfolio(string email, List<CoinEntity> portfolio)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in portfolio)
            {
                sb.AppendLine(string.Format("Coin ID: {0}", item.Id));
                sb.AppendLine(string.Format("Coin Name: {0}", item.Name));
                sb.AppendLine(string.Format("Coin Amount: {0}", item.Amount));
                sb.AppendLine(string.Format("Coin Price_USD: {0} $", item.Price_USD));
                sb.AppendLine(string.Format("Coin Rank: {0}", item.Rank));
                sb.AppendLine(string.Format("Coin Symbol: {0}", item.Symbol));
                sb.AppendLine();
            }

            byte[] fileContents = Encoding.Unicode.GetBytes(sb.ToString());
            string portfolioBlobName = createPortfolioBlobName(email);
            _cloudBlobAccess.UploadBlob(fileContents, portfolioBlobName);
        }

        public byte[] GetPortfolioFileContents(string email)
        {
            string portfolioBlobName = createPortfolioBlobName(email);
            byte[] portoflioFileContents = _cloudBlobAccess.DownloadPortfolio(portfolioBlobName);

            return portoflioFileContents;
        }

        private string createPortfolioBlobName(string email)
        {
            string portfolioBlobName = string.Format("{0}_{1}", email, "Portfolio");
            return portfolioBlobName;
        }

        public void AddCoinToUserPortfolio(string email, CoinEntity coin)
        {
            this._portfolioDataAccess.AddCoinToUserPortfolio(email, coin);
        }

        public void RefreshPortfolio(string email)
        {
            _cloudQueueAccess.PostGetPortfolioMessage(email);
        }

        public void DeletePortfolio(string email)
        {
            this._portfolioDataAccess.DeleteUserPortfolio(email);
        }
    }
}
