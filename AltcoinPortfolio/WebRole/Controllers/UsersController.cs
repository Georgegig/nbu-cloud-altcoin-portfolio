using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class UsersController : Controller
    {
        public JsonResult Username()
        {
            return this.Json(new { username = string.Empty });
        }
    }
}