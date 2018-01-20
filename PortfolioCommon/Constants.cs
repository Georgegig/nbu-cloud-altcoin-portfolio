using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortfolioCommon
{
    public class Constants
    {
        public class TableNames
        {
            public const string User = "User";
        }

        public class UserColumnNames
        {
            public const string Id = "Id";
            public const string Email = "Email";
            public const string Name = "Name";
            public const string Password = "Password";
        }

        public class CoinColumnNames
        {
            public const string Id = "Id";
            public const string PortfolioId = "PortfolioId";
            public const string Name = "Name";
            public const string Symbol = "Symbol";
            public const string Rank = "Rank";
            public const string Price_USD = "Price_USD";
            public const string Amount = "Amount";
        }

        public class QueueNames
        {
            public const string GetPortfolio = "getportfolio";
        }

        //public class BlobNames
        //{
        //    public const string DrawRaffle = "raffle";
        //}

        public class StorageModes
        {
            public const string Table = "Table";
            public const string SQLServer = "SQLServer";
        }
    }
}
