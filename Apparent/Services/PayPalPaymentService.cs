using Azure.Core;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;

namespace Apparent.Services
{
    public class PayPalPaymentService
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _baseURL;
        public PayPalPaymentService()
        {
            var config = GetConfig();
            _clientId = config["clientId"].ToString();
            _clientSecret = config["clientSecret"].ToString();
            _baseURL=GetBaseURL();
        }
        // getting properties from the web.config  
        public Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }

        // Getting access token from PayPal
        private string GetAccessToken()
        {
            string accessToken = new OAuthTokenCredential(_clientId, _clientSecret).GetAccessToken();
            return accessToken;
        }

        // return apicontext object by invoking it with the accesstoken
        private APIContext GetAPIContext()
        {
            APIContext apiContext = new APIContext(GetAccessToken());
            apiContext.Config = GetConfig();
            return apiContext;
        }
        private string GetBaseURL()
        {
            HttpContext context=HttpContext.Current;
            if (context != null )
            {
                return context.Request.Url.GetLeftPart(UriPartial.Authority) + context.Request.ApplicationPath;
            }
            else { return null; }
        }

        public Payment CreatePayment(decimal amount, string currency)
        {
            try
            {
                var payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            amount = new Amount
                            {
                                currency = currency,
                                details=new Details
                                {   
                                    tax = "0.0",
                                    subtotal="0.0"
                                },
                                total = amount.ToString("0.00"),
                            },
                            description = "Payment description",
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = _baseURL+"PayPalPayment/ExecutePayment",
                        cancel_url = _baseURL+"PayPalPayment/CancelPayment"
                    }
                };
                return payment.Create(GetAPIContext());
            }
            catch (Exception ex)
            {
                return new Payment();
            }
        }

        public Payment ExecutePayment(string paymentId, string payerId)
        {
            try
            {
                var paymentExecution = new PaymentExecution { payer_id = payerId };
                var payment = new Payment { id = paymentId };
                return payment.Execute(GetAPIContext(), paymentExecution);
            }
            catch (Exception ex)
            {
                return new Payment ();
            }
        }

        public Payment CreatePaypalPayment(decimal amount, string currency)
        {
            try
            {
                var payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer { payment_method = "paypal" },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            amount = new Amount
                            {
                                currency = currency,
                                details=new Details
                                {
                                    tax = "0.0",
                                    subtotal="0.0"
                                },
                                total = amount.ToString("0.00"),
                            },
                            description = "Payment description",
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = _baseURL + "PayPalPayment/ExecutePayPalPayment",
                        cancel_url = _baseURL + "PayPalPayment/CancelPayment"
                    }
                };
                return payment.Create(GetAPIContext());
            }
            catch (Exception ex)
            {
                return new Payment();
            }
        }
    }
}