using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioCommon.Entities
{
    public class CoinEntity
    {
        public string Id { get; set; }
        public Guid PortfolioId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public int Rank { get; set; }
        public decimal Price_USD { get; set; }
        public double Amount { get; set; }
    }
}
