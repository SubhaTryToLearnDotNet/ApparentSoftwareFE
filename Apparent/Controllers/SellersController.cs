﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apparent.Controllers
{
    public class SellersController : BaseController
    {
        // GET: Sellers
        public ActionResult Index()
        {
            return View();
        }
    }
}