using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apparent.Controllers
{
    public class BuyersController : BaseController
    {
        // GET: Buyers
        public ActionResult Index()
        {
            return View();
        }
    }
}