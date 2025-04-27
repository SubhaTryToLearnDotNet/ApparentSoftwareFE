using Apparent.DBContext;
using Apparent.DBContext.EntityDbContext;
using Apparent.DBContext.Repositroy;
using Apparent.Model;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.WebSockets;
using static System.Net.WebRequestMethods;

namespace Apparent.Controllers
{
	public class ProductController : BaseController
	{
		private readonly CompanyService _companyService;
		public ProductController()
		{
			_companyService = new CompanyService();

		}

		ProductMaster objresult = new ProductMaster();
		public async Task<ActionResult> GetProducts()
		{
			if (Session["CompanyId"] != null)
			{
				var CompanyId = Session["CompanyId"].ToString();

				CompanyDbContext dbContext = new CompanyDbContext();
				ViewModel model = new ViewModel();
				model.Products.AddRange(await dbContext.GetProductsList(CompanyId));
				model.company = await _companyService.GetCompanyById(CompanyId);
				model.company.ActionName = "GetProducts";
				return View(model);
			}
			else
			{
				return RedirectToAction("Index", "SignIn");
			}
		}
		public async Task<ActionResult> Add()
		{
			if (Session["CompanyId"] != null)
			{
				var id = Session["CompanyId"].ToString();
				CompanyDbContext company = new CompanyDbContext();
				ViewModel model = new ViewModel();
				model.company = await company.GetCompanyDetails(id);
				return View(model);
			}
			else
			{
				return RedirectToAction("Index", "SignIn");
			}
		}
		public async Task<ActionResult> AddProduct(ProductMaster ObjModel)
		{

			try

			{
				if (Session["CompanyId"] != null)
				{
					StorageService storageService = new StorageService();
					ObjModel.CompanyId = Session["CompanyId"].ToString();
					//File Upload
					DataTable dtVideo = new DataTable();
					dtVideo.Columns.Add("FileName");
					dtVideo.Columns.Add("FilePath");
					dtVideo.Columns.Add("FileSize");
					dtVideo.Columns.Add("SellerId");
					dtVideo.Columns.Add("ProductId");

					DataTable Screenshottbl = new DataTable();
					Screenshottbl.Columns.Add("Images");
					Screenshottbl.Columns.Add("ProductId");
					foreach (string file in Request.Files)
					{
						if (file != null && file.Contains("FileName"))
						{
							var filecontent = Request.Files[file];



							ObjModel.VideoFile = filecontent.FileName;

							var size = filecontent.ContentLength;
							ObjModel.FileSize = Convert.ToString(size / 1000000) + " " + "mb";

							Guid fileGuid = Guid.NewGuid();
							var FilePath = fileGuid.ToString("N").Substring(0, 12) + "." + filecontent.FileName.Split('.')[1];
							ObjModel.FilePath = FilePath;
							//filecontent.SaveAs(Server.MapPath(Path.Combine("~/Content/Web/videofiles", FilePath)));
							Stream imageStream = filecontent.InputStream;
							string containerName = "filename" + ObjModel.CompanyId;
							BlobResult imageResult = await storageService.UploadFile(imageStream, containerName, FilePath);

							dtVideo.Rows.Add(filecontent.FileName, imageResult.Uri, ObjModel.FileSize, ObjModel.SellerId, "");


						}
						else if (file != null && file.Contains("ProductImage"))
						{
							var filecontent = Request.Files[file];
							var Icon = filecontent.FileName;


							Guid fileGuid = Guid.NewGuid();
							var FilePath = fileGuid.ToString("N").Substring(0, 12) + "." + filecontent.FileName.Split('.')[1];



							//filecontent.SaveAs(Server.MapPath(Path.Combine("~/Content/Web/ProductLogo", FilePath)));

							Stream imageStream = filecontent.InputStream;
							string containerName = "producticon" + ObjModel.CompanyId;
							BlobResult imageResult = await storageService.UploadFile(imageStream, containerName, FilePath);
							ObjModel.ProductIcon = imageResult.Uri;

						}
						else if (file != null && file.Contains("ScreenshotFile"))
						{
							var filecontent = Request.Files[file];
							var FileName = filecontent.FileName;

							Guid fileGuid = Guid.NewGuid();
							var FilePath = fileGuid.ToString("N").Substring(0, 12) + "." + filecontent.FileName.Split('.')[1];
							//filecontent.SaveAs(Server.MapPath(Path.Combine("~/Content/Web/ScreenShot", FilePath)));
							Stream imageStream = filecontent.InputStream;
							string containerName = "images" + ObjModel.CompanyId;
							BlobResult imageResult = await storageService.UploadFile(imageStream, containerName, FilePath);

							Screenshottbl.Rows.Add(imageResult.Uri, "");
						}
					}

					//db Connection
					ObjModel.VideoList = dtVideo;
					ObjModel.ImagesList = Screenshottbl;
					//ObjModel.PdfFileList = PdfTbl;

					ProductDBContext dBContext = new ProductDBContext();
					var Dt = dBContext.ProductAdd(ObjModel);
					if (Dt != null && Dt.Rows.Count > 0)
					{
						string a = Dt.Rows[0]["ProductId"].ToString();


						objresult.ProductId = a;
						objresult.respons_result = "success";
					}
					else
					{
						objresult.respons_result = null;
					}

					return Json(objresult, JsonRequestBehavior.AllowGet);

				}


				else
				{
					return Json(null, JsonRequestBehavior.AllowGet);
				}
			}
			catch
			{
				return Json(null, JsonRequestBehavior.AllowGet);

			}

		}

        public ActionResult Details(string ProductId)
        {
            try
            {
                ProductDBContext obj = new ProductDBContext();
                ProductMaster model = new ProductMaster();
                var Tables = obj.ProducDetails(ProductId);
                model.DtProductDetails = Tables.Tables[0];
                model.VideoList = Tables.Tables[1];
                model.ImagesList = Tables.Tables[2];
                model.Features = Tables.Tables[3];
                model.ProductName = Tables.Tables[0].Rows[0]["ProductName"].ToString();
                foreach (DataRow dataRow in Tables.Tables[4].Rows)
                {
                    var Description = dataRow["Description"].ToString();
                    model.subscriptions.Add(new subscription
                    {
                        Plan_Name = dataRow["Plan_Name"].ToString(),
                        Plan_tenure = dataRow["Plan_tenure"].ToString(),
                        Price = dataRow["Price"].ToString(),
                        AdditionalFeatures = Description.Split('#'),
                        Plan_uniqueId = dataRow["Plan_uniqueId"].ToString(),
                        //token = dataRow["token"].ToString()
                    });
                }
                //DataTable dt = obj.GetCancelationDetailsForProductPage(ProductId);

                //if (dt.Rows.Count > 0 && dt != null)
                //{
                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        model.Link1 = dr["Link1"].ToString();
                //        model.Link2 = dr["Link2"].ToString();
                //        model.description1 = dr["description1"].ToString();
                //        model.description2 = dr["description2"].ToString();
                //        model.CancellationPolicy = dr["CancellationPolicy"].ToString();
                //        model.CancellationImage = dr["CancellationImage"].ToString();
                //        model.CancellationPDF = dr["CancellationPDF"].ToString();
                //    }
                //}
                model.Features1 = new List<string>();
                string FeaturesGroup1 = "";
                model.Features2 = new List<string>();
                string FeaturesGroup2 = "";
                model.Features3 = new List<string>();
                string FeaturesGroup3 = "";

                if (Tables.Tables[3].Rows[0]["FeaturesGroup1"] != null && !string.IsNullOrWhiteSpace(Tables.Tables[3].Rows[0]["FeaturesGroup1"].ToString()))
                {
                    //set features table to list
                    FeaturesGroup1 = Tables.Tables[3].Rows[0]["FeaturesGroup1"].ToString();
                }
                if (Tables.Tables[3].Rows[0]["Add_featuresg1"] != null && !string.IsNullOrWhiteSpace(Tables.Tables[3].Rows[0]["Add_featuresg1"].ToString()))
                {
                    string Add_features1 = Tables.Tables[3].Rows[0]["Add_featuresg1"].ToString();

                    if (FeaturesGroup1 != null && FeaturesGroup1.Trim() != string.Empty)
                    {
                        FeaturesGroup1 = FeaturesGroup1 + "& " + Add_features1;
                    }
                    else
                    {
                        FeaturesGroup1 = Add_features1;
                    }
                }


                string[] frtgrps1 = FeaturesGroup1.Split('&');
                model.Features1.AddRange(frtgrps1);

                if (Tables.Tables[3].Rows[0]["FeaturesGroup2"] != null && !string.IsNullOrWhiteSpace(Tables.Tables[3].Rows[0]["FeaturesGroup2"].ToString()))
                {


                    FeaturesGroup2 = Tables.Tables[3].Rows[0]["FeaturesGroup2"].ToString();
                }
                if (Tables.Tables[3].Rows[0]["Add_featuresg2"] != null && !string.IsNullOrWhiteSpace(Tables.Tables[3].Rows[0]["Add_featuresg2"].ToString()))
                {

                    string Add_features2 = Tables.Tables[3].Rows[0]["Add_featuresg2"].ToString();
                    if (FeaturesGroup2 != null && FeaturesGroup2.Trim() != string.Empty)
                    {
                        FeaturesGroup2 = FeaturesGroup2 + "& " + Add_features2;
                    }
                    else
                    {
                        FeaturesGroup2 = Add_features2;
                    }

                }

                string[] frtgrps2 = FeaturesGroup2.Split('&');
                model.Features2.AddRange(frtgrps2);
                if (Tables.Tables[3].Rows[0]["FeaturesGroup3"] != null && !string.IsNullOrWhiteSpace(Tables.Tables[3].Rows[0]["FeaturesGroup3"].ToString()))
                {

                    FeaturesGroup3 = Tables.Tables[3].Rows[0]["FeaturesGroup3"].ToString();
                }
                if (Tables.Tables[3].Rows[0]["Add_featuresg3"] != null && !string.IsNullOrWhiteSpace(Tables.Tables[3].Rows[0]["Add_featuresg3"].ToString()))
                {

                    string Add_features3 = Tables.Tables[3].Rows[0]["Add_featuresg3"].ToString();

                    if (FeaturesGroup3 != null && FeaturesGroup3.Trim() != string.Empty)
                    {
                        FeaturesGroup3 = FeaturesGroup3 + "& " + Add_features3;
                    }
                    else
                    {
                        FeaturesGroup3 = Add_features3;
                    }
                }

                string[] frtgrps3 = FeaturesGroup3.Split('&');
                model.Features3.AddRange(frtgrps3);
                return View(model);
            }
            catch
            {

            }
            return RedirectToAction("Index", "Home");

        }

        public ActionResult Addfeatures(string ProductId)
		{

			ViewBag.ProductId = ProductId;
			return View();

		}
		[HttpPost]
		public ActionResult features(features master)
		{
			try
			{
				master.CompanyId = Session["CompanyId"].ToString();

				ProductDBContext dBContext = new ProductDBContext();
				var table = dBContext.AddFeatures(master);

				if (table != null && table.Rows[0][0].ToString() == "success")
				{
					objresult.ProductId = master.ProductId;
					objresult.respons_result = "Success";
				}
				else
				{
					objresult.respons_result = null;
				}



				return Json(objresult, JsonRequestBehavior.AllowGet);
			}
			catch
			{
				return Json(null, JsonRequestBehavior.AllowGet);
			}
		}

		public ActionResult Addprice(string ProductId)
		{

			ViewBag.ProductId = ProductId;

			return View();





		}
		[HttpPost]
		public ActionResult Price(ProductMaster master)
		{
			try
			{
				if (Convert.ToInt32(master.Mergin) >= 10)
				{
					master.CompanyId = Session["CompanyId"].ToString();

					JavaScriptSerializer serializer = new JavaScriptSerializer();
					var subscriptionList = serializer.Deserialize<List<subscription>>(master.subcrp);

					//var token_list = serializer.Deserialize<List<subscription_token>>(master.token_master);
					//add subscription in a table

					//add column name
					DataTable DT = new DataTable();
					DT.Columns.Add("Plan_Name", typeof(string));
					DT.Columns.Add("Price", typeof(decimal));
					DT.Columns.Add("Plan_tenure", typeof(string));
					DT.Columns.Add("Description", typeof(string));
					DT.Columns.Add("subscription_comment", typeof(string));
					DT.Columns.Add("Plan_uniqueId", typeof(string));
					DT.Columns.Add("created_date", typeof(DateTime));

					//add value row by row 
					for (int i = 0; i < subscriptionList.Count; i++)
					{
						DateTime currentDate = DateTime.Today;
						DataRow DR = DT.NewRow();
						DR["Plan_Name"] = subscriptionList[i].Plan_Name;
						DR["Price"] = subscriptionList[i].Price;
						DR["Plan_tenure"] = subscriptionList[i].Plan_tenure;
						DR["Description"] = subscriptionList[i].Description;
						DR["subscription_comment"] = master.subscription_comment;
						DR["Plan_uniqueId"] = subscriptionList[i].Plan_uniqueId;
						DR["created_date"] = currentDate;
						DT.Rows.Add(DR);
					}

					//master.Token_List = token_table;
					master.Packeglist = DT;
					var ProductId = master.ProductId;
					ProductDBContext dBContext = new ProductDBContext();
					var tables = dBContext.ProductPrice(master);

					if (tables != null && tables.Rows[0][0].ToString() == "success")
					{

						var url = "/product/GetProducts";
						//return RedirectToAction("GetProducts");
						return Json(new { code = 200, msg = "Product added successful", url });


					}
					else
					{
						return Json(new { code = 400, msg = "Something wrong !" });
					}
				}
				else
				{
					return Json(new { code = 400, msg = "Minimum apparent commision is 10% !" });
				}
			}
			catch
			{
				return RedirectToAction("Add");
			}
		}

		public ActionResult TopCategory(string ProductCategory)
		{
			ProductDBContext dBContext = new ProductDBContext();

			var table = dBContext.SelectProductbyCategory(ProductCategory);
			List<ProductMaster> cdlist = new List<ProductMaster>();
			if (table != null && table.Rows.Count > 0)
			{

				for (int l = 0; l < table.Rows.Count; l++)
				{
					ProductMaster cd = new ProductMaster();

					cd.ProductId = table.Rows[l]["ProductId"].ToString();
					cd.ProductIcon = table.Rows[l]["ProductIcon"].ToString();
					cd.ProductName = table.Rows[l]["ProductName"].ToString();





					cdlist.Add(cd);
				}

			}
			return Json(cdlist, JsonRequestBehavior.AllowGet);
			//else
			//{
			//    return Json(null,JsonRequestBehavior.AllowGet);
			//}


		}

		public async Task<ActionResult> review()
		{

			if (Session["CompanyId"] != null)
			{
				var CompanyId = Session["CompanyId"].ToString();

				ProductDBContext productDBContext = new ProductDBContext();
				ViewModel model = new ViewModel();
				model.Products.AddRange(await productDBContext.ProductReview(CompanyId));
				model.company = await _companyService.GetCompanyById(CompanyId);
				model.company.ActionName = "review";
				return View(model);
			}
			else
			{
				return RedirectToAction("Index", "SignIn");
			}
		}

		public async Task<ActionResult> Remove(string productid)
		{

			if (Session["CompanyId"] != null)
			{
				var CompanyId = Session["CompanyId"].ToString();

				ProductDBContext productDBContext = new ProductDBContext();
				ViewModel model = new ViewModel();
				bool removed = await productDBContext.remove(productid);
				if (removed)
				{
					return Json(new { code = 200, msg = "Product removed from review list" });
				}
				else
				{
					return Json(new { code = 400, msg = "Something went wrong" });
				}


			}
			else
			{
				return Json(new { code = 500, msg = "Something went wrong" });
			}
		}

		public async Task<ActionResult> update(string id)
		{
			ProductDBContext productDBContext = new ProductDBContext();
			ViewModel viewModel = new ViewModel();
			viewModel.productMaster = await productDBContext.updateproduct(id);
			return View(viewModel);
		}
		[HttpPost]
		public async Task<ActionResult> updateProduct(ProductMaster ObjModel)
		{

			try

			{
				if (Session["CompanyId"] != null)

				{
					ObjModel.CompanyId = Session["CompanyId"].ToString();
					StorageService storageService = new StorageService();
					//File Upload
					DataTable dtVideo = new DataTable();
					dtVideo.Columns.Add("FileName");
					dtVideo.Columns.Add("FilePath");
					dtVideo.Columns.Add("FileSize");
					dtVideo.Columns.Add("SellerId");
					dtVideo.Columns.Add("ProductId");
					DataTable Screenshottbl = new DataTable();
					Screenshottbl.Columns.Add("Images");
					Screenshottbl.Columns.Add("ProductId");
					foreach (string file in Request.Files)
					{
						if (file != null && file.Contains("FileName"))
						{
							var filecontent = Request.Files[file];
							ObjModel.VideoFile = filecontent.FileName;

							var size = filecontent.ContentLength;
							ObjModel.FileSize = Convert.ToString(size / 1000000) + " " + "mb";

							Guid fileGuid = Guid.NewGuid();
							var FilePath = fileGuid.ToString("N").Substring(0, 12) + "." + filecontent.FileName.Split('.')[1];
							ObjModel.FilePath = FilePath;
							//filecontent.SaveAs(Server.MapPath(Path.Combine("~/Content/Web/videofiles", FilePath)));
							Stream imageStream = filecontent.InputStream;
							string containerName = "filename" + ObjModel.CompanyId;
							BlobResult imageResult = await storageService.UploadFile(imageStream, containerName, FilePath);

							dtVideo.Rows.Add(FilePath, imageResult.Uri, ObjModel.FileSize, ObjModel.SellerId, ""); ;

						}
						else if (file != null && file.Contains("ProductImage"))
						{
							var filecontent = Request.Files[file];
							var Icon = filecontent.FileName;
							Guid fileGuid = Guid.NewGuid();
							var FilePath = fileGuid.ToString("N").Substring(0, 12) + "." + filecontent.FileName.Split('.')[1];
							//filecontent.SaveAs(Server.MapPath(Path.Combine("~/Content/Web/ProductLogo", FilePath)));
							Stream imageStream = filecontent.InputStream;
							string containerName = "producticon" + ObjModel.CompanyId;
							BlobResult imageResult = await storageService.UploadFile(imageStream, containerName, FilePath);
							ObjModel.ProductIcon = imageResult.Uri;

						}
						else if (file != null && file.Contains("ScreenshotFile"))
						{
							var filecontent = Request.Files[file];
							var FileName = filecontent.FileName;
							Guid fileGuid = Guid.NewGuid();
							var FilePath = fileGuid.ToString("N").Substring(0, 12) + "." + filecontent.FileName.Split('.')[1];
							// ObjModel.Images = FilePath;

							//filecontent.SaveAs(Server.MapPath(Path.Combine("~/Content/Web/ScreenShot", FilePath)));
							Stream imageStream = filecontent.InputStream;
							string containerName = "images" + ObjModel.CompanyId;
							BlobResult imageResult = await storageService.UploadFile(imageStream, containerName, FilePath);

							Screenshottbl.Rows.Add(imageResult.Uri, "");
						}
					}
					if (ObjModel.AllBlobImgAddress != null && ObjModel.AllBlobImgAddress.Length > 0)
					{
						foreach (string img in ObjModel.AllBlobImgAddress)
						{
							if (!string.IsNullOrEmpty(img))
							{
								Screenshottbl.Rows.Add(img, "");
							}
						}
					}
					if (ObjModel.AllBlobVideoAddress != null && ObjModel.AllBlobVideoAddress.Length > 0)
					{
						foreach (string url in ObjModel.AllBlobVideoAddress)
						{
							if (!string.IsNullOrEmpty(url))
							{
								dtVideo.Rows.Add("", url, "", "", "");
							}
						}

					}
					//db Connection
					ObjModel.VideoList = dtVideo;
					ObjModel.ImagesList = Screenshottbl;
					ProductDBContext dBContext = new ProductDBContext();
					var Dt = dBContext.updatedetails(ObjModel);
					if (Dt != null && Dt.Rows.Count > 0)
					{
						string a = Dt.Rows[0]["ProductId"].ToString();
						objresult.ProductId = a;
						objresult.respons_result = "success";
					}
					else
					{
						objresult.respons_result = null;
					}
					return Json(objresult, JsonRequestBehavior.AllowGet);
				}
				else
				{
					return Json(null, JsonRequestBehavior.AllowGet);
				}
			}
			catch
			{
				return Json(null, JsonRequestBehavior.AllowGet);

			}

		}

		public async Task<ActionResult> update_features(string ProductId)
		{

			ProductDBContext productDBContext = new ProductDBContext();
			ViewModel viewModel = new ViewModel();
			viewModel.productMaster = await productDBContext.selectfeatures(ProductId);
			return View(viewModel);

		}
		[HttpPost]
		public async Task<ActionResult> udatefeaturesDetails(features master)
		{
			try
			{
				master.CompanyId = Session["CompanyId"].ToString();
				ProductDBContext dBContext = new ProductDBContext();
				bool update = await dBContext.UpdateFeatures(master);
				if (update)
				{
					var url = "/Product/price_update?ProductId=" + master.id;
					return Json(new { code = 200, msg = "Features updated successful", url });


				}
				else
				{
					return Json(new { code = 500, msg = "something wrong" });
				}



			}
			catch
			{
				return Json(null, JsonRequestBehavior.AllowGet);
			}
		}


		public async Task<ActionResult> price_update(string ProductId)
		{
			ProductDBContext dBContext = new ProductDBContext();
			ViewModel model = new ViewModel();
			model.productMaster = await dBContext.selectPriceList(ProductId);

			return View(model);
		}

		public async Task<ActionResult> price_detailsupdate(ProductMaster master)
		{
			try
			{
				if (Convert.ToInt32(master.Mergin) >= 10)
				{

					master.CompanyId = Session["CompanyId"].ToString();

					JavaScriptSerializer serializer = new JavaScriptSerializer();
					var subscriptionList = serializer.Deserialize<List<subscription>>(master.subcrp);
					//var token_list = serializer.Deserialize<List<subscription_token>>(master.token_master);
					DataTable DT = new DataTable();

					DT.Columns.Add("Plan_Name", typeof(string));
					DT.Columns.Add("Price", typeof(decimal));
					DT.Columns.Add("Plan_tenure", typeof(string));
					DT.Columns.Add("Description", typeof(string));
					DT.Columns.Add("subscription_comment", typeof(string));
					DT.Columns.Add("Plan_uniqueId", typeof(string));
					DT.Columns.Add("created_date", typeof(DateTime));

					for (int i = 0; i < subscriptionList.Count; i++)
					{
						DateTime currentDate = DateTime.Today;

						DataRow DR = DT.NewRow();

						DR["Plan_Name"] = subscriptionList[i].Plan_Name;
						DR["Price"] = subscriptionList[i].Price;
						DR["Plan_tenure"] = subscriptionList[i].Plan_tenure;
						DR["Description"] = subscriptionList[i].Description;
						DR["subscription_comment"] = master.subscription_comment;
						DR["Plan_uniqueId"] = subscriptionList[i].Plan_uniqueId;
						DR["created_date"] = currentDate;
						DT.Rows.Add(DR);
					}


					//DataTable token_table = new DataTable();

					//token_table.Columns.Add("token", typeof(string));
					//token_table.Columns.Add("product_id", typeof(string));
					//token_table.Columns.Add("plan_type", typeof(string));
					//token_table.Columns.Add("Price", typeof(decimal));
					//token_table.Columns.Add("plan_tenure", typeof(string));
					//token_table.Columns.Add("plan_uniqueid", typeof(string));

					//for (int i = 0; i < token_list.Count; i++)
					//{
					//    var tokenarray = token_list[i].token.Split(',');
					//    for (int a = 0; a < tokenarray.Length; a++)
					//    {
					//        DataRow DR1 = token_table.NewRow();
					//        DR1["token"] = tokenarray[a];
					//        DR1["product_id"] = master.id;
					//        DR1["plan_type"] = token_list[i].plan_type;
					//        DR1["Price"] = token_list[i].Price;
					//        DR1["plan_tenure"] = token_list[i].plan_tenure;
					//        DR1["plan_uniqueid"] = token_list[i].plan_uniqueid;

					//        token_table.Rows.Add(DR1);
					//    }
					//}



					master.Packeglist = DT;
					//master.Token_List = token_table;
					var ProductId = master.id;
					ProductDBContext dBContext = new ProductDBContext();
					//var tables = dBContext.ProductPrice(master)                 
					bool update = await dBContext.updateprice(master);

					if (update)
					{

						var url = "/product/review";
						//return RedirectToAction("GetProducts");
						return Json(new { code = 200, msg = "Product updated successful", url });

					}
					else
					{
						return Json(new { code = 400, msg = "Something wrong !" });
					}
				}
				else
				{
					return Json(new { code = 400, msg = "Minimum apparent commision is 10% !" });
				}

			}
			catch
			{
				return RedirectToAction("Index", "Signin");
			}
		}


		[HttpPost]
		public async Task<ActionResult> add_productcomment(Product_Comment product_)
		{
			ProductDBContext dBContext = new ProductDBContext();
			await dBContext.productcomment(product_);
			return Json(new { code = 200 });
		}

		public async Task<ActionResult> Another(string productId, string PlanId)
		{
			string compId = Session["CompanyId"].ToString();
			ProductDBContext product = new ProductDBContext();
			ViewModel model = new ViewModel();

			ProductMaster Master = await product.GetProductWithCompany(compId, productId, PlanId);
			var result = await SendDataToAppProduct(Master);
			return Redirect(result.ReturnURL);
		}
		public async Task<APIResponseModel> SendDataToAppProduct(ProductMaster companyMaster)
		{
			try
			{
				string apiUrl = companyMaster.Product_Url + ConfigurationManager.AppSettings["ProductAccess"].ToString();
				using (HttpClient httpClient = new HttpClient())
				{
					var requestData = new AccessMeRequestModel
					{
						UserInfo = new UserInfo()
						{
							FirstName = "",
							LastName = "",
							Email = companyMaster.Company_Email,
							//UserId = Convert.ToInt32(companyMaster.productMaster.CompanyId)
						},
						Subscription = new Subscription
						{
							ProductId = "0",
							PlanId = "0",
							PlanName = companyMaster.GetSubscription.Plan_Name,
						}
					};
					httpClient.DefaultRequestHeaders.Add("Authorization", companyMaster.token);
					string data = JsonConvert.SerializeObject(requestData);
					StringContent content4 = new StringContent(data, Encoding.UTF8, "application/json");
					HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content4);
					var res = await response.Content.ReadAsStringAsync();
					APIResponseModel resModel = JsonConvert.DeserializeObject<APIResponseModel>(res);
					return resModel;
				}
			}
			catch (Exception e) { return null; }

		}
	}
}
