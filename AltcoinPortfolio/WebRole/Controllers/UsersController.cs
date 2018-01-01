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

        public JsonResult GetUser(Guid userId)
        {
            UserEntity user = this._userManager.GetUser(userId);
            return this.Json(user);
        }
    }
}