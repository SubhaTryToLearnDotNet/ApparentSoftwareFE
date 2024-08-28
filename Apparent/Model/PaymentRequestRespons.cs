using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrueArtAMS.Entity
{
    public class PaymentRequestRespons
    { 
        public int statusCode { get; set; }
        public string status { get; set; }
        public object message { get; set; }
        public object errorDetails { get; set; }
        public string redirectUrl { get; set; }
    }
    public class PaymentSuccessRespons
    {
        public string message { get; set; }
        public bool success { get; set; }
        public string user_name { get; set; }
        public string OrderId {  get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string email { get; set; }
        public string transaction_id { get; set; }
        public string settlement_date { get; set; }
        public string payment_type { get; set; }
        public DateTime transaction_date { get; set; }
    }



}
