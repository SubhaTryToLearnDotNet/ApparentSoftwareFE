using Apparent.DBContext;
using Apparent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apparent.Controllers
{
    public class BuyersController : BaseController
    {
        // GET: Buyers
        public ActionResult Index()
        {
            try
            {
                ProductDBContext product = new ProductDBContext();
                ProductMaster model = new ProductMaster();
                var dataSet = product.ProductList();
                if (dataSet != null)
                {
                    var dt1 = dataSet.Tables[0];

                    List<string> splitValuesList = new List<string>();

                    for (int i = 0; dt1.Rows.Count > i; i++)
                    {
                        string columnValue = dt1.Rows[i]["category_name"].ToString();



                        splitValuesList.Add(columnValue);
                    }

                    model.Getcategory = splitValuesList;
                    model.DtProductList = dataSet.Tables[1];
                    return View(model);
                }
                else
                {
                    return View();
                }
            }
            catch
            {
                return View();
            }

        }
    }
}