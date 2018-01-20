using PortfolioCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioCommon.Interfaces
{
    public interface IPortfolioDataAccess
    {
        UserEntity GetUser(Guid userId);
        void RegisterUser(UserEntity user);
        string GetUserPassword(string email);
        void ClearData();

        List<CoinEntity> GetUserPortfolio(string email);
        void AddCoinToUserPortfolio(string email, CoinEntity coin);
    }
}
