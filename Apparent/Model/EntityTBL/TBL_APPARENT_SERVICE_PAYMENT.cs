using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apparent.Model.EntityTBL
{
    public class TBL_APPARENT_SERVICE_PAYMENT
    {
        [Key]
        public long Id {  get; set; }
        public string card_holder { get; set; }
        public string email { get; set; }

        public string UserName {  get; set; }
        public string card_number { get; set; }
        public string card_type { get; set; }
        public string card_category { get; set; }
        public decimal decimal_amount { get; set; }
        public string currency { get; set; }
        public string reference { get; set; }
        
        public bool successful { get; set; }
        public string message { get; set; }

        public string Notes { get; set; }   
        public string transaction_id { get; set; }
        public string respons_id {  get; set; }
        public string settlement_date { get; set; }
        public DateTime? transaction_date { get; set; }
        
    }
}