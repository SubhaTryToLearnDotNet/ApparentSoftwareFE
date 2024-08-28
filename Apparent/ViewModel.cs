using Apparent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apparent
{
    public class ViewModel
    {
        public ViewModel()
        {
            Products = new List<ProductMaster>();
            Category = new List<Category>();
            payment_master = new PaymentMaster();
            company = new CompanyMaster();
        }
        public List<ProductMaster> Products  { get; set; }
        public List<Category> Category { get; set; }
        public CompanyMaster company { get; set; }
         public ProductMaster productMaster { get; set; }
        public PaymentMaster payment_master { get; set; }

    }
}