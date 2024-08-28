using Apparent.DBContext;
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
    public class SalesController : BaseController
    {
        private readonly CompanyService _companyService;

        public SalesController() {
            _companyService = new CompanyService();
        }    
        // GET: Sales
        public async Task <ActionResult> Index()
        {
            if (Session["CompanyId"] != null)
            {
                var CompanyId = Session["CompanyId"].ToString();

                ProductDBContext productDBContext = new ProductDBContext();
                ViewModel model = new ViewModel();
                model.Products.AddRange(await productDBContext.ProdctList(CompanyId));
                model.company = await _companyService.GetCompanyById(CompanyId);
                model.company.ActionName = "Sales";
                return View(model);
            }
            else
            {
                return RedirectToAction("Index", "SignIn");
            }
        }

        public async Task<ActionResult> product_earnings(int productid)
        {
            ProductMaster productMaster = new ProductMaster();
            ProductDBContext dBContext = new ProductDBContext();
            productMaster = await dBContext.GetProduct_Sales_Details(productid);
            return View(productMaster);
        }

    }
}