using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apparent.Model
{
    public class PaymentMaster
    {

        public PaymentMaster() {

            InternationalPayment = new List<string>();
            CryptoPayment = new List<string>();
            CountryWisePayment = new List<CountryWisePayment>(); 
        }

        public List<string> InternationalPayment { get; set; }
        public List<string> CryptoPayment { get; set; }
         public List<CountryWisePayment>  CountryWisePayment { get; set; }
        public string ProductId { get; set; }
        public string Plan_Name { get; set; }
        public string Price { get; set; }
        public string Plan_tenure { get; set; }
        public string PlanId { get; set; } = string.Empty;
        public  string token { get; set; }
    }

    public class CountryWisePayment
    {
        public string Payment_Name { get; set; }
        public string  Payment_icon { get; set; }
        public  string Country_code { get; set; }
    }

    public class CardPaymentMaster
    {
        public string card_token { get; set; }

        public decimal amount { get; set; }
        public string reference { get; set; }
        public string currency { get; set; }
        public string customer_ip { get; set; }
        public string card_number { get; set; }
        public string card_holder { get; set; }
        public string card_expiry { get; set; }
        public string cvv { get; set; }
        public string month { get; set; }
        public string year { get; set; }
    }
    public class CardPaymentRespons
    {
        public bool successful { get; set; }
        public Response response { get; set; }
        public object[] errors { get; set; }
        public bool test { get; set; }
    }

    public class Response
    {
        public string authorization { get; set; }
        public string id { get; set; }
        public string card_number { get; set; }
        public string card_holder { get; set; }
        public string card_expiry { get; set; }
        public string card_token { get; set; }
        public string card_type { get; set; }
        public string card_category { get; set; }
        public string card_subcategory { get; set; }
        public int amount { get; set; }
        public decimal decimal_amount { get; set; }
        public bool successful { get; set; }
        public string message { get; set; }
        public string reference { get; set; }
        public string currency { get; set; }
        public string transaction_id { get; set; }
        public string settlement_date { get; set; }
        public DateTime? transaction_date { get; set; }
        public string response_code { get; set; }
        public bool captured { get; set; }
        //public int captured_amount { get; set; }
        public string rrn { get; set; }
        public string cvv_match { get; set; }


    }
   public class SubscriptionRespons
    {

        public string ProductId { get; set; }
        public string CompanyId { get; set; }
        public decimal Amount { get; set; }
        public string  Plan_Type { get; set; }
        public string Plan_Tenure { get; set; }
        public string Transaction_id { get; set; }
        public string Payment_Type { get; set; }
        public DateTime Settlement_date { get; set; }
        public DateTime Subscription_Expired { get; set; }
        public string Subscription_Status { get; set; }
        public string token { get; set; }
    }

    public class subscription_token
    {
       
        public string token { get; set; }
        public string product_id { get; set; }
        public string plan_type { get; set;}
        public decimal Price { get; set; }
        public string plan_tenure { get; set;}
        public string plan_uniqueid { get; set; }
    }

    public class InvoiceModel
    {
        public string  UserName { get; set; }
        public decimal decimal_amount { get; set; }
        public string transaction_id { get; set; }
        public string settlement_date { get; set; }
        public string card_category { get; set; }
        public DateTime? transaction_date { get; set; }
    }
}