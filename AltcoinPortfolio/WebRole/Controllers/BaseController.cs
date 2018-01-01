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
        protected UserManager _userManager;

        public BaseController()
        {
            this.InitializeUserManager();
        }

        private void InitializeUserManager()
        {
            IUserDataAccess userDataAccess = null;
            if(ConfigurationManager.AppSettings["StorageMode"] == Constants.StorageModes.SQLServer)
            {
                userDataAccess = new SqlServerDataAccess();

            }
            else
            {
                throw new NotImplementedException();
            }

            CloudQueueAccess queueAccess = new CloudQueueAccess();
            CloudBlobAccess blobAccess = new CloudBlobAccess();

            this._userManager = new UserManager(userDataAccess, queueAccess, blobAccess);
        }
    }
}