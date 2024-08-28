using Apparent.DBContext;
using Apparent.DBContext.Repositroy;
using Apparent.Model;
using Apparent.Repository;
using Apparent.Services;
using Newtonsoft.Json;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Apparent.Controllers
{
    public class PayPalPaymentController : Controller
    {
        private PayPalPaymentService _payPalService;
        private readonly IApiPaymentService _apiPaymentService;
        private readonly PaymentService _paymentService;
        public PayPalPaymentController()
        {
            _payPalService = new PayPalPaymentService();
            _apiPaymentService = new ApiPaymentService();
            _paymentService = new PaymentService();
        }
        // GET: PayPalPayment
        public ActionResult Index()
        {
            
            return View();
        }        
        public async Task< ActionResult> CreatePayment(string ProductId,string Plan_Type,string Plan_Tenure)
        {
            if (Session["CompanyId"] != null)
            {

                PaymentContext paymentContext = new PaymentContext();
                string CompanyId = Session["CompanyId"].ToString();
                PaymentMaster master = new PaymentMaster();
                master = await paymentContext.GetProductPricebyPlanType(ProductId, Plan_Type, Plan_Tenure);

                var amount = master.Price;
                var currency = "USD";
                TempData["ProductId"] = ProductId;
                TempData["Plan_Type"] = Plan_Type;
                TempData["Plan_Tenure"] = Plan_Tenure;

                var payment = _payPalService.CreatePayment(Convert.ToDecimal(amount), currency);
                if (payment != null)
                {
                    var redirectUrl = payment.links.FirstOrDefault(l => l.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase))?.href;
                    return Json(new { redirectTo = redirectUrl }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { redirectTo = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { redirectTo = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ExecutePayment(string paymentId, string token, string PayerID)
        {
            PaymentContext paymentContext = new PaymentContext();
            var payment = _payPalService.ExecutePayment(paymentId, PayerID);

            if (payment.state.ToLower() == "approved")
            {
                // Payment was successful
                SubscriptionRespons subrespons = new SubscriptionRespons();
                subrespons.Transaction_id = payment.id;
                //subrespons.Settlement_date = payment.create_time;
                subrespons.CompanyId = Session["CompanyId"].ToString();
                subrespons.ProductId = TempData["ProductId"].ToString();
                subrespons.Payment_Type = "PayPal";
                //subrespons.Amount = payment.amount;
                subrespons.Plan_Type = TempData["Plan_Type"].ToString();
                subrespons.Plan_Tenure = TempData["Plan_Tenure"].ToString();
                if (subrespons.Plan_Tenure == "Per Month ")
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
                var result = paymentContext.GetSubscriptionPaymentRespons(subrespons);
                TempData["PaypalSuccessTID"] = payment.id;
                TempData["PaypalSuccessDateCreated"] = payment.create_time;
                return RedirectToAction("SuccessPayment", "PayPalPayment");
            }
            else
            {
                // Payment failed
                return RedirectToAction("Failed", "Home");
            }
        }
        public ActionResult SuccessPayment()
        {
            return View();
        }
        public ActionResult CancelPayment(string paymentId, string token, string PayerID)
        {
            return View();
        }
        public async Task<ActionResult>CreateApiPayPayment(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                TempData["PaymentRequestKey"] = key;
                var respons = _apiPaymentService.CheckRequestKey(key);
                var amount = respons.Price;
                var currency = "USD";
                var payment = _payPalService.CreatePaypalPayment(Convert.ToDecimal(amount), currency);
                if (payment != null)
                {
                    var redirectUrl = payment.links.FirstOrDefault(l => l.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase))?.href;
                    return Json(new { redirectTo = redirectUrl }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { redirectTo = "" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { redirectTo = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult ExecutePayPalPayment(string paymentId, string token, string PayerID)
        {
            PaymentContext paymentContext = new PaymentContext();
            var payment = _payPalService.ExecutePayment(paymentId, PayerID);

            if (payment.state.ToLower() == "approved")
            {
                string  key = TempData["PaymentRequestKey"].ToString();
                var respons1 = _apiPaymentService.CheckRequestKey(key);
                RedirectRespons respons = new RedirectRespons();
                respons.transaction_id = payment.id;
                respons.currency = "USD";
                respons.email = respons1.Email;
                respons.amount = respons1.Price;
                respons.payment_type = "PayPal";
               var result =  _apiPaymentService.AddAppPayRespons(respons);
                string responsJson = JsonConvert.SerializeObject(respons);
                string encodedResponsJson = HttpUtility.UrlEncode(responsJson);
                string redirectUrl = respons1.RedirectUrl + "?data=" + encodedResponsJson;
                return Redirect(redirectUrl);
            }
            else
            {
                return RedirectToAction("Failed", "Home");
            }
        }
    }
}