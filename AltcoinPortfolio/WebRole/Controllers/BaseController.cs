using PortfolioCommon;
using PortfolioCommon.Access;
using PortfolioCommon.Interfaces;
using PortfolioCommon.Managers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class BaseController : Controller
    {
        protected PortfolioManager _portfolioManager;

        public BaseController()
        {
            this.InitializePortfolioManager();
        }

        private void InitializePortfolioManager()
        {
            IPortfolioDataAccess portfolioDataAccess = null;
            if(ConfigurationManager.AppSettings["StorageMode"] == Constants.StorageModes.SQLServer)
            {
                portfolioDataAccess = new SqlServerDataAccess();
            }
            else
            {
                throw new NotImplementedException();
            }

            CloudQueueAccess queueAccess = new CloudQueueAccess();
            CloudBlobAccess blobAccess = new CloudBlobAccess();

            this._portfolioManager = new PortfolioManager(portfolioDataAccess, queueAccess, blobAccess);
        }
    }
}