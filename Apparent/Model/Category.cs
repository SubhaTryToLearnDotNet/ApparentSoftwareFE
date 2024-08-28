using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apparent.Model
{
    public class Category
    {
        public long category_id { get; set; }
        public string category_name { get; set; }
        public string status { get; set;}
        public string created_date { get; set; }
        public string cat_suggestion { get; set; }
        public string suggestion_IsActive { get; set; }
        public string companyid { get; set; }
    }
}