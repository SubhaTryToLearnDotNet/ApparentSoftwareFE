using Apparent.DBContext;
using Apparent.Model;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Apparent.Controllers
{
    public class SellerController : Controller
    {
        SellerMaster modelObj = new SellerMaster();
        // GET: Seller
        public ActionResult List()
        {
            try
            {
                var dbcontext =  new SellerMasterDbContex();
                var table = dbcontext.SellerList();
                if (table != null & table.Rows.Count > 0)
                {
                    modelObj.SellerList = table;
                    return View(modelObj);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }

                
            }
            catch
            {
                TempData["NetworkeErrorMSg"] = "Something went wrong, try after some times";
                return RedirectToAction("Index", "Home");
            }
        }


        public ActionResult SellerProduct( string SellerId )
        {
            try
            {
                
                
                  

                 ProductDBContext product = new ProductDBContext();
                    ProductMaster model = new ProductMaster();
                    
                     var tables   = product.sellerProductList(SellerId);
                    model.DtProductList = tables.Tables[0];
                    model.SellerDeatils = tables.Tables[1];

                    return View(model);

                
                

            }
            catch
            {
                return RedirectToAction("SellerLogin","Login");
            }
        }

        public ActionResult AddProduct()
        {
            return View();
        }
        public ActionResult SellerDetails()
        {
            string UserId = "1";
            SellerMasterDbContex context = new SellerMasterDbContex();
            modelObj.SellerDt = context.SellerDeatils(UserId);


            return View(modelObj);
        }
        [HttpPost]
        public ActionResult UpdateDetails(SellerMaster model)
        {
            try
            {
                foreach (string file in Request.Files)

                {
                    string fileName = Path.GetFileNameWithoutExtension(model.SellerPic.FileName);
                    string Extension = Path.GetExtension(model.SellerPic.FileName);
                    fileName = fileName + Extension;
                    model.Imagepath = ("/Images/") + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    model.SellerPic.SaveAs(fileName);

                }
                SellerMasterDbContex seller = new SellerMasterDbContex();
                DataTable DT = seller.UpdateSellerDeatils(model);
                  if(DT!=null && DT.Rows[0]["result"].ToString()== "Update Successful")
                {
                    modelObj.Result = "Update Successful";        
                }
                else if(DT != null && DT.Rows[0]["result"].ToString() == "Password Update Successful")
                {
                    modelObj.Result = "Password Update Successful";
                }

                else if (DT != null && DT.Rows[0]["result"].ToString() == "Enter wrong password")
                {
                    modelObj.Result = "Enter wrong password";
                }

                else
                {
                    modelObj.Result = null;
                }

                return Json(modelObj, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            
        }

        public ActionResult SignUp()
        {
            

            return View();
        }

      
       

       

       
        
    }   
}