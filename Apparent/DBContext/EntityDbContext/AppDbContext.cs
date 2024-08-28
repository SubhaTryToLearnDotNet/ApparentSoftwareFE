using Apparent.Model;
using Apparent.Model.EntityTBL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Apparent.DBContext.EntityDbContext
{
    public class AppDbContext: DbContext
    {
        public AppDbContext() : base("name=abcd") { }

        public DbSet<Tbl_Company> Tbl_Company { get; set; }
        public DbSet<TBL_APPARENT_SERVICE_PAYMENT> TBL_APPARENT_SERVICE_PAYMENT {  get; set; }  
    }
}