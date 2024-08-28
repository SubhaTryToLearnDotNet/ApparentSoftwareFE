using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apparent.Model
{
    public class CryptoPaymentData
    {
        public int id { get; set; }
        public string plan { get; set; }
        public string tenure { get; set; }
        public string price { get; set; }
       public string PaymentRequestKey {  get; set; }
        public string email {  get; set; }
    }
}