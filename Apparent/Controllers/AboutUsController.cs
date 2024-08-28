using Apparent.DBContext;
using Apparent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apparent.Controllers
{
    public class AboutUsController : BaseController
    {
        // GET: AboutUs
        public ActionResult Index()
        {
            return View();
        }
    }
}