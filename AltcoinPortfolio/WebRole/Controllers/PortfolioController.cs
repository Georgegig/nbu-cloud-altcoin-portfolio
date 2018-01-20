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
            //get portfolio

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
    }
}