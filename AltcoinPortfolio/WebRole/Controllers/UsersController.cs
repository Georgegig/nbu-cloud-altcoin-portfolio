using PortfolioCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController() : base()
        {
        }
        [HttpPost]
        public JsonResult LoginUser(UserEntity user)
        {
            object result = null;

            string passFromDb = this._portfolioManager.GetUserPassword(user.Email);
            if (!string.IsNullOrEmpty(passFromDb))
            {
                if (passFromDb != user.Password)
                {
                    return Json(new { success = false, result }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    result = new { user.Email };
                }
            }
            else
            {
                return Json(new { success = false, result }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUser(Guid userId)
        {
            UserEntity user = this._portfolioManager.GetUser(userId);
            if (user != null)
            {
                return this.Json(user, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return this.Json(new { empty = "Empty" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult RegisterUser(UserEntity user)
        {
            try
            {
                this._portfolioManager.RegisterUser(user);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = "Error occured while trying to register user" });
            }

            return Json(new { success = true, message = "User registered" });
        }
    }
}