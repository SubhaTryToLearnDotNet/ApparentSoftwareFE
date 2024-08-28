using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Apparent.Model
{
    public class ProductSalseResponse
    {
        public string status { get; set; }

        public int status_code { get; set; }
        public int  company_id { get; set; }
        public  string company_name { get; set; }
        public string email { get; set; } = string.Empty;

        public decimal total_amount { get; set; }
        public decimal apparent_commission { get; set; }
        public Product[] product { get; set; }
        public string error { get; set; }
    }

    public class Product
    {
        public int product_id { get; set; }
        public string product_name { get; set; }
        public decimal Sales { get; set; }  
        public decimal product_commssion {  get; set; }
        public Data[] data { get; set; }
    }
    public class Data
    {
        public int tranasaction_id { get; set; }
        public string customer_name { get; set; }
        public decimal amount { get; set; }
        public string plan { get; set; }
        public string tenure { get; set; }
        public string tranasaction_date { get; set; }
    }
}