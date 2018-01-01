using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebRole.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController() : base()
        {               
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}