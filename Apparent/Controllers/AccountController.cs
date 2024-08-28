using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apparent.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult SwitchAccount(string Accountvalue)
        {
            if (!string.IsNullOrEmpty(Accountvalue))
            {
                if(Accountvalue == "Seller")
                {
                    TempData["Type"] = "Seller";
                    return Json(new { Msg = "Seller", url = "/Profile" });
                    
                }
                else
                {
                    TempData["Type"] = "Customer";
                    return Json(new { Msg = "Customer", url = "/Profile/customerprof" });
                   

                }
            }
            else
            {
                return View();
            }

            
        }
    }
}