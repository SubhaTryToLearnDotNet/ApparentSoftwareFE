using Apparent.DBContext;
using Apparent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Apparent.Controllers
{
    public class BaseController : Controller
    {
        protected override async void OnActionExecuting(ActionExecutingContext filterContext)
        {
           
           
            if (Session["CompanyId"]!= null)
            {
                var Companyid = Session["CompanyId"].ToString();
                CompanyDbContext context =  new CompanyDbContext();
                ViewBag.CompanyId = Companyid;
                CompanyMaster company = await context.FindCompany(Companyid);
                 if (company != null)
                {
                    Session["CompanyName"] = company.CompanyName;
                    Session["CompanyEmail"] = company.CompanyEmail;
                    Session["LastName"] = company.Last_Name;
                    var AccountType = company.Type;
                    Session["CompanyIcon"] = company.CompanyIcon;
                    Session["FirstName"] = company.First_Name+" "+company.Last_Name;
                    

                    if (TempData["Type"] != null && TempData["Type"].ToString() == "Seller")
                    {
                        ViewBag.Account = "Both";
                        ViewBag.Type = TempData["Type"].ToString();
                        Session["Type"] ="Seller";
                    }
                    else if(TempData["Type"] != null && TempData["Type"].ToString() == "Customer")
                    {
                        ViewBag.Account = "Both";
                        ViewBag.Type = TempData["Type"].ToString();
                        Session["Type"] = "Customer"; 
                   }
                    else
                    {
                        if (AccountType != null && AccountType == "Seller")
                        {
                            ViewBag.Type = "Seller";
                            Session["Type"] = "Seller";
                        }
                        else if (AccountType != null && AccountType == "Customer")
                        {
                            ViewBag.Type = "Customer";
                            Session["Type"] = "Customer";
                        }
                        else if (AccountType != null && AccountType == "Both")
                        {
                            ViewBag.Account = "Both";
                            if (Session["Type"] == null)
                            {
                                Session["Type"] = "Seller";
                            }
                            if (Session["Type"] == null)
                            {
                                Session["Type"] = "Seller";
                            }
                        }
                        
                    }
                        
                }
            }
           if(Session["Country_Code"] != null)
            {
                TempData["country_code"] = Session["Country_Code"].ToString();
            }

        }
    }
}