using Apparent.DBContext.EntityDbContext;
using Apparent.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Apparent.DBContext.Repositroy
{
    public class CompanyService
    {
        private readonly AppDbContext _appDbContext;
        public CompanyService() 
        { 
            _appDbContext =  new AppDbContext();
        }
        public async Task<CompanyMaster> GetCompanyById(string Id)
        {
            try
            {
                int companyId = Convert.ToInt32(Id);
                var company = await _appDbContext.Tbl_Company
           .Where(x => x.CompanyId == companyId)
           .FirstOrDefaultAsync();

                if (company != null)
                {

                    CompanyMaster companyMaster = new CompanyMaster
                    {

                        CompanyId = company.CompanyId.ToString(),
                        CompanyName = company.CompanyName,
                        CompanyIcon = company.CompanyIcon,
                        First_Name = company.First_Name,
                        Last_Name = company.Last_Name,


                    };

                    return companyMaster;
                }

                return null;

            }
            catch
            {
                return null;
            }
        }
        public async Task<CompanyMaster> GetCompanyByEmail(string email)
        {
            try
            {
                var company = await _appDbContext.Tbl_Company
             .Where(x => x.CompanyEmail == email)
             .FirstOrDefaultAsync();

                if (company != null)
                {

                    CompanyMaster companyMaster = new CompanyMaster
                    {

                        CompanyId = company.CompanyId.ToString(),
                        CompanyName = company.CompanyName,
                        CompanyIcon = company.CompanyIcon,
                        First_Name = company.First_Name,
                        Last_Name = company.Last_Name,


                    };

                    return companyMaster;
                }

                return null;

            }
            catch
            {
                return null;
            }
        }

        public async Task<bool>updatepassword(SignInModel model)
        {
            try
            {
                var company = await _appDbContext.Tbl_Company
             .Where(x => x.CompanyEmail == model.Email)
             .FirstOrDefaultAsync();

                if (company != null)
                {
                    company.Password = model.Password;

                   await _appDbContext.SaveChangesAsync();
                    return true;
                }
             
                return false;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}