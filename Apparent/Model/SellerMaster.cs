using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Apparent.Model
{
    public class SellerMaster :Country
    {
        public string SellerWebsite
        {
            get;set;
        }

        public string  SellerId { get; set; }
        public string FirstName { get; set; }
        public string  LastName { get; set; }
        public string  Email { get; set; }
        public string Password { get; set; }
        public string Contact { get; set; }
        public string CurrentPassword { get; set; }
        public string Result { get; set; }  
        public string About { get; set; }
        public DataTable SellerList { get; set; }   
        public HttpPostedFileBase SellerPic { get; set; }
        public string Imagepath { get; set; }

       public DataTable SellerDt { get; set; }
        public List<Country> CountryList { get; set; }
    }
    public class Country
    {
        public long Id { get; set; }
        public string Country_Name { get; set; }
    }
}