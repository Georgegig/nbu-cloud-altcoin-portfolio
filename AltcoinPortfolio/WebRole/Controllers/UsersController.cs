using PortfolioCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

            /* Fetch the stored value */
            string passFromDb = this._portfolioManager.GetUserPassword(user.Email);
            if (!string.IsNullOrEmpty(passFromDb))
            {
                /* Extract the bytes */
                byte[] hashBytes = Convert.FromBase64String(passFromDb);
                /* Get the salt */
                byte[] salt = new byte[16];
                Array.Copy(hashBytes, 0, salt, 0, 16);
                /* Compute the hash on the password the user entered */
                var pbkdf2 = new Rfc2898DeriveBytes(user.Password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);
                /* Compare the results */
                for (int i = 0; i < 20; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                        return Json(new { success = false, result }, JsonRequestBehavior.AllowGet);
                }

                result = new { user.Email };                
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