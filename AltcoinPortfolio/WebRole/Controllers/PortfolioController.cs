using PortfolioCommon.DataModels;
using PortfolioCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class PortfolioController : BaseController
    {
        public PortfolioController() : base()
        {
        }

        public JsonResult GetUserPortfolio(string email)
        {            
            List<CoinEntity> result = new List<CoinEntity>();            

            result = this._portfolioManager.GetUserPortfolio(email);

            return Json(new { result }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddCoin(EmailCoinDataModel coin)
        {

            try
            {
                this._portfolioManager.AddCoinToUserPortfolio(coin.UserEmail, coin.Coin);
            }
            catch (Exception e)
            {
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }
        
        public JsonResult DeletePortfolio(string email)
        {
            try
            {
                this._portfolioManager.DeletePortfolio(email);
                NotificationHub.Reload();
            }
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RefreshPortfolio(string email)
        {
            try
            {
                this._portfolioManager.RefreshPortfolio(email);
                NotificationHub.Reload();
            }
            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadPortfolio(string email)
        {
            byte[] fileBytes = GetPortfolio(email);
            return File(
                fileBytes,
                 "application/x-msdownload", string.Format("User{0}_Portfolio.txt", email));
        }

        byte[] GetPortfolio(string email)
        {
            try
            {
                return _portfolioManager.GetPortfolioFileContents(email);
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}