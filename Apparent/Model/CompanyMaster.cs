﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apparent.Model
{
    public class CompanyMaster
    {
        public CompanyMaster() {

            Category_list = new List<string>(); 
        }
        public string CompanyId  { get; set; }
        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Type { get; set; }
        public string Fax { get; set; }
        public string land_no { get; set; }
        public string CompanyIcon { get; set; }
        public string CompanyName { get; set; }
        public string HQLocation { get; set; }
        public string CompanyEmail { get; set; }
        public string CompanyWebsite { get; set; }
        public string Twitter { get; set; }
        public string LinkedIn { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Contact_Person { get; set; }
        public string Person_Position { get; set; }
        public string Alter_Contact { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string country_code { get; set; }
        public string Zip { get; set; }
        public List<string> Category_list { get; set; }
        public HttpPostedFileBase CompanyImage { get; set; }
        public string Current_Password { get; set; }
        public string Password { get; set; }
        public string result { get; set; }
        public string Token2 { get; set; }

        public string ActionName{ get; set; }
    }
}