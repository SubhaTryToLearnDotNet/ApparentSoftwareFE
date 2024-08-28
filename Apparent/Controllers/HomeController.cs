using Apparent.DBContext;
using Apparent.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Collections;
//using MaxMind.Db;
//using MaxMind.GeoIP2;
using System.Net;

namespace Apparent.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Home
        public async Task <ActionResult> Index()
        
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

                    for(int i=0; dt1.Rows.Count>i;i++)
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