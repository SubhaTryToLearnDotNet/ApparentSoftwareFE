using Apparent.DBContext;
using Apparent.DBContext.Repositroy;
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
    public class CategoryController : BaseController
    {
        private readonly CompanyService _companyService;
        public CategoryController() {
            _companyService = new CompanyService();
        }
        // GET: Category
        public async Task< ActionResult >Index()
        {
            if (Session["CompanyId"] != null && Session["Type"].ToString()=="Seller")
            {
                ViewModel viewModel = new ViewModel();
                CategoryDbContex dbContex = new CategoryDbContex();
                viewModel.Category.AddRange(await  dbContex.select_category(Session["CompanyId"].ToString()));
                viewModel.company = await _companyService.GetCompanyById(Session["CompanyId"].ToString());
                viewModel.company.ActionName = "Category";
                return View(viewModel);
            }
            else
            {
              return  RedirectToAction("Index","SignIn");
            }
        }
        [HttpPost]
        public async Task<ActionResult> add_category(Category obj )
        {
            if (Session["CompanyId"] != null && Session["Type"].ToString() == "Seller")
            {
                CategoryDbContex categoryDb =  new CategoryDbContex();
                bool category = await categoryDb.add_category(obj.category_name, Session["CompanyId"].ToString());
                if (category) 
                {
                    return Json(new { code = 200, msg = "The Category will show once the Admin approves the category." });
                }
                else
                {
                    return Json(new { code = 400, msg = "Something went wrong!"});
                }              
            }
            else
            {
                return RedirectToAction("Index", "SignIn");
            }
        }
        public async Task<ActionResult> update_category(Category obj)
        {
            if (Session["CompanyId"] != null && Session["Type"].ToString() == "Seller")
            {
                CategoryDbContex categoryDb = new CategoryDbContex();
                bool update = await categoryDb.update_category(obj);
                if (update)
                {
                    return Json(new { code = 200, msg = "Category updated succseeful." });
                }
                else
                {
                    return Json(new { code = 400, msg = "Something went wrong!" });
                }
            }
            else
            {
                return RedirectToAction("Index", "SignIn");
            }
        }
    }
}