using Apparent.DBContext;
using Apparent.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Apparent.Controllers
{
    public class SignUpController : Controller
    {
        // GET: SignUp
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Submit(CompanyMaster obj)
        {
            try
            {
                CompanyDbContext context = new CompanyDbContext();
                var model = await context.SignUp(obj);
                if (model.result == "Email Send successfull")
                {

                    return RedirectToAction("verification", "SignUp", new { id = model.CompanyId });
                }
                else if (model.result == "Email already exsist")
                {
                    ViewBag.Message = "Email already exsist..";
                    return View("index");
                }
                else if (model.result == "Error")
                {
                    ViewBag.Message = "Something went wrong..";
                    return View("index");
                }
                else
                {
                    TempData["CompanyExist"] = model.result;
                    return RedirectToAction("Index", "LogIn");
                }

            }
            catch
            {
                return View("SignUp");
            }
        }

        public ActionResult customer_signup()
        {
            return View();
        }

        public async Task<ActionResult> customer_submit(CompanyMaster obj)
        {
            try
            {
                CompanyDbContext context = new CompanyDbContext();
                var model = await context.customer_signup(obj);
                if (model.result == "Email Send successfull")
                {

                    return RedirectToAction("verification", "SignUp", new { id = model.CompanyId});
                }
                else
                {
                    TempData["CompanyExist"] = model.result;
                    return RedirectToAction("Index", "SignIn");
                }

            }
            catch
            {
                return RedirectToAction("customer_submit", "SignUp");
            }
        }




        public ActionResult verification(string id)
        {
            ViewBag.CompanyId = id;
           
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> verify(string EmailVerificationCode, string CompanyId)
        {
            try
            {
                CompanyDbContext context = new CompanyDbContext();
                var verify = await context.EmailVerify(CompanyId, EmailVerificationCode);

                if (verify.result == "Email verified successfully")
                {
                   
                        string url = "/SignIn";
                        return Json(new { code = 200, url });
                   
                }
                else
                {
                   
                    return Json(new { code = 400});
                }
            }
            catch
            {
                return View();
            }

        }

        public async Task<ActionResult> ResendEmailVerificationCode(string CompanyId)
        {
            

            if (!string.IsNullOrEmpty(CompanyId))
            {
                Random random = new Random();
                var emailverificationCode = Convert.ToString(random.Next(10000000, 99999999));

                CompanyDbContext dbContext = new CompanyDbContext();
                 string email = await dbContext.UpdateVerificationCode(CompanyId, emailverificationCode);
                if (!string.IsNullOrEmpty(email))
                {
                    EmailService emailService = new EmailService();
                    emailService.Emailverification(email, emailverificationCode);

                    return Json(new { code = 200 });
                }
                else
                {
                    return Json(new { code = 500 });
                }
            }
            else
            {
                string url = "/SignIn";
                return Json(new { code = 400, url });
            }
        }
    }
}