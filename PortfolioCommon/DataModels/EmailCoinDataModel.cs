using PortfolioCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioCommon.DataModels
{
    public class EmailCoinDataModel
    {
        public string UserEmail { get; set; }
        public CoinEntity Coin { get; set; }
    }
}
