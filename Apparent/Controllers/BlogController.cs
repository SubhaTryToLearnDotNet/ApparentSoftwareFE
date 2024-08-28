using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apparent.Controllers
{
    public class BlogController : BaseController
    {
        // GET: BlogDetails
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BlogDetails()
        {
            return View();
        }
    }
}