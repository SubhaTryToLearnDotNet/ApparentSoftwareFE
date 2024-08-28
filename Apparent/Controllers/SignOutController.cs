using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Apparent.Controllers
{
    public class SignOutController : Controller
    {
        // GET: SignOut
        public async Task<ActionResult> Index()
        {
            if (Session["CompanyId"] != null)
            {
                Session.Clear();
                Session.Abandon();
                return RedirectToAction("Index", "SignIn");
            }
            return RedirectToAction("Index", "SignIn");
        }
    }
}