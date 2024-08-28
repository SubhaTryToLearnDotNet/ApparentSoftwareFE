using Apparent.DBContext.Repositroy;
using Apparent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Apparent.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly CompanyService _companyService;
       public ResetPasswordController() {
            _companyService = new CompanyService();
        } 
        public ActionResult Index(string encpid)
        {
            if (!string.IsNullOrEmpty(encpid))
            {
                ViewBag.Outh = encpid;
                return View();

            }
            return RedirectToAction("Index", "SignIn");
            
        }
        [HttpPost]
        public async Task<ActionResult>password_update(SignInModel signIn)
        {
            try
            {
                bool Reset = await _companyService.updatepassword(signIn);
                if (Reset)
                {

                    return Json(new { code = 200, msg = "Password updated successfully", url = "/SignIn" });
                }

                return Json(new { code = 404, msg = "Email is not valid", url = "/SignIn" });
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Password updated successfully", url = "/SignIn" }); ;
            }
        }
    }
}