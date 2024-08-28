using Apparent.DBContext;
using Apparent.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Apparent.DBContext.Repositroy;
using System.Web.Http.Results;
using Apparent.Services;
using Apparent.Repository;

namespace Apparent.Controllers
{
    public class ServiceController : Controller
    {
        private readonly PaymentService _paymentService;
        private readonly IApiPaymentService _apiPaymentService;
        private readonly string _PaymentTokenHeaderauthorization;
        public ServiceController()
        {
            _paymentService = new PaymentService();
            _apiPaymentService = new ApiPaymentService();
            _PaymentTokenHeaderauthorization = ConfigurationManager.AppSettings.Get("PaymentTokenHeaderauthorization");
        }
        public ActionResult Payment()
        {
            CardMasterModel cardMasterModel = new CardMasterModel();
            return View(cardMasterModel);
        }

        public async Task<ActionResult> card_payment_submit(CardMasterModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _paymentService.PaymentSubmit(model);

                    if (result != null && (result.successful == true && result.status == "Approved"))
                    {
                        return Json(new { code = 200, msg = "Your payments is being processed.", url = "/Service/invoice_receipt?trans_id=" + result.tetransaction_id });

                    }
                    else if (result != null && (result.successful == false && result.status == "Declined"))
                    {
                        return Json(new { code = 400, msg = "Declined.(Refer to card issuer)" });
                    }
                    else
                    {
                        return Json(new { code = 500, msg = result.message });
                    }
                }
                else
                {
                    return Json(new { code = 400, msg = "Validation failed." });
                }
            }
            catch
            {
                return Json(new { code = 500, msg = "Internal server error!" });
            }
        }

        [HttpGet]
        public async Task<ActionResult> invoice_receipt(string trans_id)
        {
            if (!string.IsNullOrEmpty(trans_id))
            {
                var respons = await _paymentService.Get_invoice(trans_id);
                if (respons != null)
                {
                    return View(respons);
                }
                else
                {
                    return View();
                }
            }
            else
            {
                return View();
            }

        }

        public ActionResult Pay(string Key)
        {
            if (string.IsNullOrEmpty(Key))
            {
                return RedirectToAction("Index", "Home");
            }
            var respons = _apiPaymentService.CheckRequestKey(Key);
            if (respons != null)
            {
                if (!IsRequestValid(respons.PaymentRequestKey))
                {
                    return RedirectToAction("Index", "Home");
                }
                return View(respons);

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            
        }

        [HttpPost]
        public ActionResult CryptoIndex(CryptoPaymentData model)
        {
                return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> card_payment(CardMasterModel model,string key)
        {
            var respons = _apiPaymentService.CheckRequestKey(key);
              model.Amount = respons.Price;
            var result = await _paymentService.PaymentSubmit(model);
            if (result != null && (result.successful == true && result.status == "Approved"))
            {
                RedirectRespons respons1 = new RedirectRespons();
                var RESC = await _paymentService.Get_invoice(result.tetransaction_id);
                respons1.amount = RESC.decimal_amount;
                respons1.transaction_id = result.tetransaction_id;
                respons1.email = respons.Email;
                respons1.currency = "AUD";
                respons1.transaction_date = RESC.transaction_date;
                respons1.payment_type = "Card";
                respons1.message = "success";
                respons1.success = true;
                _apiPaymentService.AddAppPayRespons(respons1);
                string responsJson = JsonConvert.SerializeObject(respons1);
                string encodedResponsJson = HttpUtility.UrlEncode(responsJson);
                string redirectUrl = respons.RedirectUrl + "?data=" + encodedResponsJson;
                return Json(new { code = 200, msg = "Your payments is being processed.", RedirectUrl = redirectUrl });
            }
            else if (result != null && (result.successful == false && result.status == "Declined"))
            {
                RedirectRespons respons1 = new RedirectRespons();
                respons1.amount = respons.Price;
                respons1.message = "failed";
                respons1.success = false;
                string responsJson = JsonConvert.SerializeObject(respons1);
                string encodedResponsJson = HttpUtility.UrlEncode(responsJson);
                string redirectUrl = respons.RedirectUrl + "?data=" + encodedResponsJson;
                return Json(new { code = 400, msg = "Declined.(Refer to card issuer)" , RedirectUrl = redirectUrl });
            }
            else
            {
                return Json(new { code = 500, msg = result.message });
            }
        }
        



        public static bool IsRequestValid(string key)
        {

            string[] parts = key.Split('_');

            if (long.TryParse(parts[1], out long ticks))
            {
                DateTime timestamp = new DateTime(ticks);
                TimeSpan timeDifference = DateTime.Now - timestamp;
                if (timeDifference.TotalMinutes <= 30)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false ;
            }
        }
    }
}