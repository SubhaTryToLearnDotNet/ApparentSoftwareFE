using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apparent.Model
{
    public class CardMasterModel
    {
        public string User_Name { get; set; }
        public string CardNumber { get; set; }
        public string CardholderName { get; set; }
        public string ExpirationDate { get; set; }
        public string CVV { get; set; }
        public decimal Amount { get; set; }
        public string Notes { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string Email { get; set; }
        public string message {  get; set; }
        public string status { get; set; }
        public string tetransaction_id { get; set; }
        public bool successful { get; set; }

    }
}