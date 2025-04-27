using Apparent.DBContext;
using Apparent.Model;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TrueArtAMS.Entity;

namespace Apparent.Controllers
{
	public class PaymentController : BaseController
	{


		private readonly string _PaymentTokenHeaderauthorization;
		public PaymentController()
		{

			_PaymentTokenHeaderauthorization = ConfigurationManager.AppSettings.Get("PaymentTokenHeaderauthorization");
		}

		// GET: Payment
		public async Task<ActionResult> Index(string ProductId, string Plan_Name, decimal Price, string Plan_tenure, string PlanId)
		{
			#region Session Condition
			if (Session["CompanyId"] == null && (Session["Type"] == null && Session["Type"].ToString() != "Customer"))
			{
				return RedirectToAction("Index", "SignIn");
			}
			if (Session["Country_Code"] == null) { return RedirectToAction("customerprof", "Profile"); }
			#endregion

			#region Payment Page View

			if (Price != 0)
			{
				string CompanyId2 = Session["CompanyId"].ToString();
				string FirstName = Session["FirstName"].ToString();
				string Last_Name = Session["LastName"].ToString();
				string CompanyEmail = Session["CompanyEmail"].ToString();
				ViewModel model = new ViewModel();
				PaymentContext paymentContext = new PaymentContext();
				model.payment_master.Price = Convert.ToString(Price);
				model.company.First_Name = FirstName;
				model.company.Last_Name = Last_Name;
				model.company.CompanyEmail = CompanyEmail;
				model.company.CompanyId = CompanyId2;
				model.payment_master.ProductId = ProductId;
				model.payment_master.Plan_Name = Plan_Name;
				model.payment_master.PlanId = PlanId;
				string IdentityKey = paymentContext.SubmitSubcriptionDetails(model);

				string baseUrl = $"https://saas.apparent.world";
				string baseUrlLocal = $"https://localhost:44349";
				string relativeUrl = Url.Action("Submit", "Payment");
				string redirectUrl1 = $"{baseUrl}{relativeUrl}";

				PaymentRequestModel requestModel = new PaymentRequestModel
				{
					Price = Price,
					Email = CompanyEmail,
					RedirectUrl = redirectUrl1,
					ProductName = "Apparent",
					IdentityKey = IdentityKey
				};

				using (HttpClient httpClient = new HttpClient())
				{

					string apiUrl = "https://my.apparentpay.com/api/ApparentPay/Payment";
					//string apiUrl = "https://localhost:44365/api/ApparentPay/Payment";
					string data = JsonConvert.SerializeObject(requestModel);
					StringContent content4 = new StringContent(data, Encoding.UTF8, "application/json");
					httpClient.Timeout = TimeSpan.FromSeconds(30);
					HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content4);
					if (response.IsSuccessStatusCode)
					{
						string PaymentContent = await response.Content.ReadAsStringAsync();
						PaymentRequestRespons paymentRespons = JsonConvert.DeserializeObject<PaymentRequestRespons>(PaymentContent);
						if (paymentRespons.status == "success")
						{
							var redirectUrl = paymentRespons.redirectUrl;
							return Redirect(redirectUrl);
						}
						else
						{
							return Json(new { code = 400, msg = "Payment request failed" });
						}
					}
					else
					{
						return Json(new { code = 400, msg = "Payment API request failed" });
					}
				}
			}

			#endregion

			#region Get Company Details & Products,Plan Deatail & Prepare Data

			var CompanyId = Session["CompanyId"].ToString();
			ProductDBContext db2 = new ProductDBContext();
			ProductMaster product = await db2.GetProductWithCompany(CompanyId, ProductId, PlanId);
			APIResponseModel Response = new APIResponseModel();
			#endregion

			#region Call Third Party Api

			if (product.result != "0") { Response = await UpdateSubscrition(product); }
			else { Response = await SendDataToAppProduct(product); }
			#endregion

			#region Prepare Response Data & Return
			if (Response.StatusCode != 200)
			{
				TempData["FreeTrialErrorMSG"] = "Internal Server Error. Try After Some Times";
				return RedirectToAction("Details", "Product", new { ProductId = ProductId });
			}
			DateTime currentDate = DateTime.Now;
			DateTime newDate = currentDate.AddDays(13);
			PaymentContext paymentContext1 = new PaymentContext();
			bool Isactive = await paymentContext1.Sold_FreeToken(Session["CompanyId"].ToString(), ProductId, newDate, currentDate, PlanId);
			TempData["FreeTrialMSG"] = "Congratulations!Product Free Trial is Activate";
			return RedirectToAction("my_product", "Profile");
			#endregion

		}
		public async Task<ActionResult> card_payment(CardPaymentMaster paymentMaster, string ProductId, string Plan_Type, string Plan_Tenure, string token, string PlanId)
		{
			try
			{
				if (Session["CompanyId"] != null)
				{
					PaymentContext paymentContext = new PaymentContext();
					string CompanyId = Session["CompanyId"].ToString();
					PaymentMaster master = new PaymentMaster();
					master = await paymentContext.GetProductPricebyPlanType(ProductId, Plan_Type, Plan_Tenure);
					paymentMaster.amount = Convert.ToDecimal(Convert.ToInt32(master.Price) * 100);
					var paymentrespons = await card_payment_submit(paymentMaster, CompanyId, ProductId, Plan_Type, Plan_Tenure, PlanId);
					if (paymentrespons != null && paymentrespons.Rows.Count > 0)
					{
						DateTime currentDateTime = DateTime.Now;
						SubscriptionRespons subrespons = new SubscriptionRespons();
						subrespons.Transaction_id = paymentrespons.Rows[0]["transaction_id"].ToString();
						subrespons.Settlement_date = currentDateTime;
						subrespons.CompanyId = Session["CompanyId"].ToString();
						subrespons.ProductId = ProductId;
						subrespons.Payment_Type = paymentrespons.Rows[0]["card_type"].ToString();
						subrespons.Amount = Convert.ToDecimal(Convert.ToInt32(master.Price));
						subrespons.Plan_Type = Plan_Type;
						subrespons.Plan_Tenure = Plan_Tenure;
						subrespons.token = token;
						if (subrespons.Plan_Tenure == "Per Month")
						{
							DateTime nextMonthDate = subrespons.Settlement_date.AddMonths(1).AddDays(-1);
							subrespons.Subscription_Expired = nextMonthDate;
						}
						else if (subrespons.Plan_Tenure == "Per Year ")
						{
							DateTime nextMonthDate = subrespons.Settlement_date.AddYears(1).AddDays(-1); ; ;

							subrespons.Subscription_Expired = nextMonthDate;
						}
						else
						{
							DateTime nextMonthDate = subrespons.Settlement_date.AddYears(5).AddDays(-1); ; ;

							subrespons.Subscription_Expired = nextMonthDate;
						}
						var result = await paymentContext.GetSubscriptionPaymentRespons(subrespons);

						TempData["SubscriptionUpdateMsg"] = "Subscription updated successfully.";
						return Json(new { code = 200, msg = "Payment Successfully Done." });
					}
					else
					{
						if (TempData["PaymentMsg"] != null)
						{
							string msg = TempData["PaymentMsg"].ToString();
							return Json(new { code = 200, msg = msg });
						}
						return Json(new { code = 500, msg = "Internal Server Error.Try After Some Times." });
					}
				}
				else
				{
					var url = "/SignIn";
					return Json(new { code = 500, url });
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				var url = "/SignIn";
				return Json(new { code = 500, url });
			}
		}
		public async Task<DataTable> card_payment_submit(CardPaymentMaster model, string CompanyId, string ProductId, string Plan_Type, string Plan_Tenure, string PlanId)
		{
			try
			{
				#region Get IP Address
				DateTime currentDateTime = DateTime.Now;
				string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
				var ip = "";
				string hostName = Dns.GetHostName();
				IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);
				foreach (IPAddress ipAddress in ipAddresses)
				{
					ip = ipAddress.ToString();
				}
				#endregion

				#region Prepare Data To Call Payment Api
				DataTable table = new DataTable();
				CardPaymentRespons paymentRespons = new CardPaymentRespons();

				CardPaymentMaster paymentsubmit = new CardPaymentMaster();
				paymentsubmit.card_expiry = model.month + "/" + model.year;
				paymentsubmit.card_number = model.card_number;
				paymentsubmit.card_holder = model.card_holder;
				paymentsubmit.cvv = model.cvv;
				paymentsubmit.customer_ip = ip;
				paymentsubmit.amount = model.amount;
				paymentsubmit.reference = formattedDateTime + "/" + model.card_number;
				paymentsubmit.currency = "AUD";

				#endregion

				#region Call Third Party API 

				string apiUrl = ConfigurationManager.AppSettings["FatzebraURL"].ToString();
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Add("accept", "application/json");
					httpClient.DefaultRequestHeaders.Add("authorization", _PaymentTokenHeaderauthorization);

					string data = JsonConvert.SerializeObject(paymentsubmit);

					StringContent content4 = new StringContent(data, Encoding.UTF8, "application/json");

					HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content4);

					if (!response.IsSuccessStatusCode)
					{
						return table;
					}
					string PaymentContent = await response.Content.ReadAsStringAsync();
					paymentRespons = JsonConvert.DeserializeObject<CardPaymentRespons>(PaymentContent);

					if (paymentRespons.successful != true && paymentRespons.response.message != "Approved")
					{
						return table;
					}
				}
				#endregion

				#region Prepare Payment  Data
				Response response1 = new Response();

				response1.authorization = paymentRespons.response.authorization;
				response1.id = paymentRespons.response.id;
				response1.decimal_amount = paymentRespons.response.decimal_amount;
				response1.card_token = paymentRespons.response.card_token;
				response1.card_type = paymentRespons.response.card_type;
				response1.reference = paymentRespons.response.reference;
				response1.card_number = paymentRespons.response.card_number;
				response1.transaction_date = Convert.ToDateTime(paymentRespons.response.transaction_date);
				response1.settlement_date = paymentRespons.response.settlement_date;
				response1.transaction_id = paymentRespons.response.transaction_id;
				response1.currency = paymentRespons.response.currency;
				response1.message = paymentRespons.response.message;
				response1.rrn = paymentRespons.response.rrn;
				response1.successful = paymentRespons.response.successful;

				#endregion

				#region Call Subscription Update Api & Register Api

				ProductDBContext db2 = new ProductDBContext();
				//CompanyMaster CompanyMaster = await db.GetCompanyDetails(CompanyId);
				ProductMaster Outputs = await db2.GetProductWithCompany(CompanyId, ProductId, PlanId);
				APIResponseModel ApiResponse = new APIResponseModel();

				if (Outputs.result != "0") { ApiResponse = await UpdateSubscrition(Outputs); }
				else { ApiResponse = await SendDataToAppProduct(Outputs); }

				#endregion

				#region Return Data
				if (ApiResponse.StatusCode == 200)
				{
					TempData["PaymentMsg"] = paymentRespons.response.message;
					PaymentContext context = new PaymentContext();
					DataTable Dt = await context.Submit_CardPayment_Response(response1, CompanyId, ProductId, Plan_Type, Plan_Tenure);
					return Dt;
				}
				TempData["ErrorMSG"] = "Internal Server Error. Try After Some Times";
				return table;
				#endregion
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
				return null;
			}
		}

		public async Task<APIResponseModel> SendDataToAppProduct(ProductMaster dsProduct)
		{
			try
			{
				#region Prepare Data
				APIResponseModel ApiResponse = new APIResponseModel();
				AccessMeRequestModel ApiRequest = new AccessMeRequestModel
				{
					UserInfo = new UserInfo()
					{
						FirstName = dsProduct.First_Name,
						LastName = dsProduct.Last_Name,
						Email = dsProduct.Company_Email,
						//UserId = Convert.ToInt32(companyMaster.productMaster.CompanyId)
					},
					Subscription = new Subscription
					{
						ProductId = "Convert.ToInt32(dsProduct.ProductId),",
						PlanId = "Convert.ToInt32(dsProduct.GetSubscription.Plan_uniqueId",
						PlanName = dsProduct.GetSubscription.Plan_Name,

					}
				};
				#endregion

				#region Call Third Party API
				string apiUrl = dsProduct.Product_Url + ConfigurationManager.AppSettings["ProductRegister"].ToString();
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Add("Authorization", dsProduct.token);
					string data = JsonConvert.SerializeObject(ApiRequest);
					StringContent content4 = new StringContent(data, Encoding.UTF8, "application/json");
					HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content4);
					string ApiResponseContent = await response.Content.ReadAsStringAsync();
					ApiResponse = JsonConvert.DeserializeObject<APIResponseModel>(ApiResponseContent);
				}
				#endregion

				#region Prepare Response & Retuen
				if (ApiResponse.StatusCode != 200)
				{
					ApiResponse.StatusCode = 500;
					ApiResponse.Message = ApiResponse.Message;
					return ApiResponse;
				}
				return ApiResponse;
				#endregion
			}
			catch (Exception e) { Console.WriteLine(e.ToString()); return null; }
		}

		public async Task<APIResponseModel> UpdateSubscrition(ProductMaster dsProduct)
		{
			try
			{
				#region Prepare Data 
				APIResponseModel ApiResponse = new APIResponseModel();
				var requestData = new AccessMeRequestModel
				{
					UserInfo = new UserInfo()
					{
						FirstName = dsProduct.First_Name,
						LastName = dsProduct.Last_Name,
						Email = dsProduct.Company_Email,
						//UserId = Convert.ToInt32(companyMaster.productMaster.CompanyId)
					},
					Subscription = new Subscription
					{
						ProductId = "0",
						PlanId = "0",
                        PlanName = dsProduct.GetSubscription.Plan_Name,
					}
				};
				#endregion

				#region Call Third Party API
				string apiUrl = dsProduct.Product_Url + ConfigurationManager.AppSettings["SubscriptionUpdate"].ToString();
				using (HttpClient httpClient = new HttpClient())
				{
					httpClient.DefaultRequestHeaders.Add("Authorization", dsProduct.token);
					string data = JsonConvert.SerializeObject(requestData);
					StringContent content4 = new StringContent(data, Encoding.UTF8, "application/json");
					HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content4);
					string ApiResponseContent = await response.Content.ReadAsStringAsync();
					ApiResponse = JsonConvert.DeserializeObject<APIResponseModel>(ApiResponseContent);
				}
				#endregion

				#region Prepare Response & Return Data

				if (ApiResponse.StatusCode != 200)
				{
					ApiResponse.StatusCode = 500;
					ApiResponse.Message = ApiResponse.Message;
					return ApiResponse;
				}
				return ApiResponse;

				#endregion
			}
			catch (Exception e) { Console.WriteLine(e.ToString()); return null; }

		}

		[HttpPost]
		public ActionResult CryptoIndex(CryptoPaymentData model)
		{
			if (Session["CompanyId"] == null && (Session["Type"] == null && Session["Type"].ToString() != "Customer"))
			{
				return RedirectToAction("Index", "SignIn");

			}
			return View(model);
		}


	}
}