using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Threading.Tasks;
using Apparent.Model;
using System.ComponentModel.Design;

namespace Apparent.DBContext
{
    public class CompanyDbContext
    {
        public CompanyDbContext() {
   
        }
        private string Cs = ConfigurationManager.ConnectionStrings["abcd"].ConnectionString;
        public async Task<CompanyMaster>SignUp(CompanyMaster obj)
        {
            CompanyMaster company = new CompanyMaster();
            try
            {
                Random random = new Random();
                var emailverificationCode   = Convert.ToString( random.Next(10000000, 99999999)); 
                var token=Guid.NewGuid().ToString().Replace("-","");
                //DataBase Operation
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Company", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyName", obj.CompanyName);
                cmd.Parameters.AddWithValue("@CompanyEmail", obj.CompanyEmail);
                cmd.Parameters.AddWithValue("@Country", obj.Country);
                cmd.Parameters.AddWithValue("@Password", obj.Password);
                cmd.Parameters.AddWithValue("@EmailVeryfication", emailverificationCode);
                cmd.Parameters.AddWithValue("@Token", token);
                cmd.Parameters.AddWithValue("@Task", "CompanySignUp");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                EmailService emailService = new EmailService();
                
                if (dt.Rows.Count > 0) {

                    if ( dt.Rows[0]["result"].ToString() == "success")
                    {
                       await emailService.Emailverification(obj.CompanyEmail, emailverificationCode);
                        company.result = "Email Send successfull";
                    }
                    else if ( dt.Rows[0]["result"].ToString() == "Company not verify")
                    {
                        await emailService.Emailverification(obj.CompanyEmail, emailverificationCode);
                        company.result = "Email Send successfull";
                    }
                    else
                    {
                        company.result = "Email already exsist";
                    }

                    company.CompanyName = dt.Rows[0]["CompanyName"].ToString();

                    company.CompanyId = dt.Rows[0]["CompanyId"].ToString();

                }
                return company;
            }
            catch
            {
                company.result = "Error";
                return company;
            }

        }




        public async Task<CompanyMaster> customer_signup(CompanyMaster obj)
        {
            try
            {
                Random random = new Random();

                var emailverificationCode = Convert.ToString(random.Next(10000000, 99999999));
                //emailverificationCode = Convert.ToString(98765432);
                //DataBase Operation
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Company", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@First_Name", obj.First_Name);
                cmd.Parameters.AddWithValue("@Last_Name", obj.Last_Name);
                cmd.Parameters.AddWithValue("@CompanyEmail", obj.CompanyEmail);
                cmd.Parameters.AddWithValue("@Country", obj.Country);
                cmd.Parameters.AddWithValue("@Password", obj.Password);
                cmd.Parameters.AddWithValue("@EmailVeryfication", emailverificationCode);
                cmd.Parameters.AddWithValue("@Task", "customer_signup");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                EmailService emailService = new EmailService();
                CompanyMaster company = new CompanyMaster();
                if (dt.Rows.Count > 0)
                {

                    if (dt.Rows[0]["result"].ToString() == "success")
                    {
                       await emailService.Emailverification(obj.CompanyEmail, emailverificationCode);
                        company.result = "Email Send successfull";
                    }
                    else if (dt.Rows[0]["result"].ToString() == "Company not verify")
                    {
                       await emailService.Emailverification(obj.CompanyEmail, emailverificationCode);
                        company.result = "Email Send successfull";
                    }
                    else
                    {
                        company.result = "Customer email already exsist";
                    }

                    company.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                    company.CompanyEmail = dt.Rows[0]["CompanyEmail"].ToString();
                    company.CompanyId = dt.Rows[0]["CompanyId"].ToString();

                }
                return company;
            }
            catch
            {
                return null;
            }

        }


        public async Task<CompanyMaster> EmailVerify(string CompanyId, string EmailVerificationCode)
        {
            try
            {
                DataTable dt = new DataTable(); 
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Company", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                cmd.Parameters.AddWithValue("@EmailVeryfication", EmailVerificationCode);
                cmd.Parameters.AddWithValue("@Task", "EmailVerify");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
               CompanyMaster companyMaster = new CompanyMaster();
                 if(dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["result"].ToString()== "Email verified successfully")
                    {
                        companyMaster.CompanyId = dt.Rows[0]["CompanyId"].ToString();
                        companyMaster.result = dt.Rows[0]["result"].ToString();
                    }
                    else
                    {
                        companyMaster.result = dt.Rows[0]["result"].ToString();
                    }

                }
                 return companyMaster;
                
            }
            catch
            {
                return null;
            }
        }
        public DataTable SignIn(CompanyMaster master)
        {
            try
            {

                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Company", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();


                cmd.Parameters.AddWithValue("@CompanyEmail", master.CompanyEmail);

                cmd.Parameters.AddWithValue("@Password", master.Password);

                cmd.Parameters.AddWithValue("@Task", "CompanySignin");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                return dt;


            }
            catch
            {
                return null;
            }
        }

        public async Task<List<ProductMaster>> GetProductsList(string CompanyId)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Company", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);

                cmd.Parameters.AddWithValue("@Task", "ProductList");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                List<ProductMaster> list = new List<ProductMaster>();
                foreach (DataRow dr in dt.Rows)
                {
                    ProductMaster product = new ProductMaster
                    {

                        ProductIcon = dr["ProductIcon"].ToString(),
                        ProductId = dr["ProductId"].ToString(),
                        ProductName = dr["ProductName"].ToString(),
                        CompanyName = dr["CompanyName"].ToString(),
                        CompanyIcon = dr["CompanyIcon"].ToString()
                    };
                    list.Add(product);
                }
                return list;
            }
            catch
            {
                return null;
            }
        }

        public async Task<CompanyMaster> GetCompanyDetails(string CompanyId)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Company", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                cmd.Parameters.AddWithValue("@Task", "CompanyDetails");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                con.Close();
                CompanyMaster companyMaster = new CompanyMaster();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    companyMaster.CompanyId = ds.Tables[0].Rows[0]["CompanyId"].ToString();
                    companyMaster.CompanyName = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                    companyMaster.CompanyEmail = ds.Tables[0].Rows[0]["CompanyEmail"].ToString();
                    companyMaster.Twitter = ds.Tables[0].Rows[0]["Twitter"].ToString();
                    companyMaster.LinkedIn = ds.Tables[0].Rows[0]["LinkedIn"].ToString();
                    companyMaster.CompanyWebsite = ds.Tables[0].Rows[0]["CompanyWebsite"].ToString();
                    companyMaster.HQLocation = ds.Tables[0].Rows[0]["HQLocation"].ToString();
                    companyMaster.Address = ds.Tables[0].Rows[0]["Address"].ToString();
                    companyMaster.Person_Position = ds.Tables[0].Rows[0]["Person_Position"].ToString();
                    companyMaster.Contact_Person = ds.Tables[0].Rows[0]["Contact_Person"].ToString();
                    companyMaster.Phone = ds.Tables[0].Rows[0]["Phone"].ToString();
                    companyMaster.Alter_Contact = ds.Tables[0].Rows[0]["Alter_Contact"].ToString();
                    companyMaster.CompanyIcon = ds.Tables[0].Rows[0]["CompanyIcon"].ToString();
                    companyMaster.City = ds.Tables[0].Rows[0]["City"].ToString();
                    companyMaster.Zip= ds.Tables[0].Rows[0]["Zip"].ToString();
                    companyMaster.Country = ds.Tables[0].Rows[0]["Country"].ToString();
                    companyMaster.State = ds.Tables[0].Rows[0]["State"].ToString();
                    companyMaster.CompanyIcon = ds.Tables[0].Rows[0]["CompanyIcon"].ToString();
                    companyMaster.Last_Name = ds.Tables[0].Rows[0]["Last_Name"].ToString();
                    companyMaster.First_Name = ds.Tables[0].Rows[0]["First_Name"].ToString();
                    companyMaster.Fax = ds.Tables[0].Rows[0]["Fax"].ToString();
                    companyMaster.land_no = ds.Tables[0].Rows[0]["land_no"].ToString();
                    companyMaster.Token2 = ds.Tables[0].Rows[0]["Token"].ToString();

                }
                if(ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows) {
                        companyMaster.Category_list.Add(dr["category_name"].ToString()

                        );
                            }
                }
              
                return companyMaster;

            }
            catch
            {
                return null;
            }
        }
        public async Task<bool> UpdateDetails(CompanyMaster obj,string CompanyId)
        {
            try
            {
                
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Company", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyName", obj.CompanyName);
                cmd.Parameters.AddWithValue("@Phone", obj.Phone);
                cmd.Parameters.AddWithValue("@Alter_Contact", obj.Alter_Contact);
                cmd.Parameters.AddWithValue("@LinkedIn", obj.LinkedIn);
                cmd.Parameters.AddWithValue("@Contact_Person", obj.Contact_Person);
                cmd.Parameters.AddWithValue("@Address", obj.Address);
                cmd.Parameters.AddWithValue("@Person_Position", obj.Person_Position);
                cmd.Parameters.AddWithValue("@CompanyEmail", obj.CompanyEmail);
                cmd.Parameters.AddWithValue("@CompanyWebsite", obj.CompanyWebsite);
                cmd.Parameters.AddWithValue("@Twitter", obj.Twitter);
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                cmd.Parameters.AddWithValue("@City", obj.City);
                cmd.Parameters.AddWithValue("@State", obj.State);
                cmd.Parameters.AddWithValue("@Zip", obj.Zip);
                cmd.Parameters.AddWithValue("@Fax", obj.Fax);
                cmd.Parameters.AddWithValue("@land_no", obj.land_no);
                cmd.Parameters.AddWithValue("@Country", obj.Country);
                cmd.Parameters.AddWithValue("@country_code", obj.country_code);
                cmd.Parameters.AddWithValue("@Task", "UpdateDetails");
              
                int i =  cmd.ExecuteNonQuery();
                con.Close();
                if(i>0) { 
                return true;
                
                }

                else
                {
                    return false;
                }

                

            }
            catch
            {
                return false ;
            }
        }
        public async Task<bool> uploadIcon(CompanyMaster obj)
        {
            try
            {

                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Company", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyIcon",obj.CompanyIcon);
                cmd.Parameters.AddWithValue("@CompanyId", obj.CompanyId);              
                cmd.Parameters.AddWithValue("@Task", "uploadicon");

                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;

                }

                else
                {
                    return false;
                }



            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> password_reset(CompanyMaster obj, string CompanyId)
        {
            try
            {

                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Company", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@Current_Password", obj.Current_Password);
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                cmd.Parameters.AddWithValue("@Password", obj.Password);
                cmd.Parameters.AddWithValue("@Task", "resetpassword");

                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;

                }

                else
                {
                    return false;
                }



            }
            catch
            {
                return false;
            }
        }
        public async Task<bool> customerprofiledetailupdate(CompanyMaster obj, string CompanyId)
        {
            try
            {

                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Company", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();                
                cmd.Parameters.AddWithValue("@CompanyEmail", obj.CompanyEmail);
                cmd.Parameters.AddWithValue("@First_Name", obj.First_Name);
                cmd.Parameters.AddWithValue("@Last_Name", obj.Last_Name);
                cmd.Parameters.AddWithValue("@Phone", obj.Phone);
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                cmd.Parameters.AddWithValue("@City", obj.City);
                cmd.Parameters.AddWithValue("@State", obj.State);
                cmd.Parameters.AddWithValue("@Zip", obj.Zip);
                cmd.Parameters.AddWithValue("@Country", obj.Country);
                cmd.Parameters.AddWithValue("@Fax", obj.Fax);
                cmd.Parameters.AddWithValue("@land_no", obj.land_no);
                cmd.Parameters.AddWithValue("@country_code", obj.country_code);
                cmd.Parameters.AddWithValue("@Task", "customer_profileupdate");

                int i = cmd.ExecuteNonQuery();
                con.Close();
                if (i > 0)
                {
                    return true;

                }

                else
                {
                    return false;
                }



            }
            catch
            {
                return false;
            }
        }

        public async Task<CompanyMaster> FindCompany(string id)
        {
            try
            {
               
                //DataBase Operation
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Company", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyId", id);
                
                cmd.Parameters.AddWithValue("@Task", "FindCompany");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                
                CompanyMaster company = new CompanyMaster();
                if (dt.Rows.Count > 0)
                {
                    company.CompanyName = dt.Rows[0]["CompanyName"].ToString();
                    company.CompanyEmail = dt.Rows[0]["CompanyEmail"].ToString();
                    company.CompanyId = dt.Rows[0]["CompanyId"].ToString();
                    company.CompanyIcon = dt.Rows[0]["CompanyIcon"].ToString();
                    company.First_Name = dt.Rows[0]["First_Name"].ToString();
                    company.Last_Name = dt.Rows[0]["Last_Name"].ToString();
                    company.Type = dt.Rows[0]["Type"].ToString();
                }
                return company;
            }
            catch
            {
                return null;
            }

        }
        public async Task<string> UpdateVerificationCode(string CompanyId,string emailverificationCode)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Company", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                cmd.Parameters.AddWithValue("@EmailVeryfication", emailverificationCode);
                cmd.Parameters.AddWithValue("@Task", "UpdateVerificationCode");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                if (dt!=null && dt.Rows.Count> 0)
                {
                    var email = dt.Rows[0]["CompanyEmail"].ToString();

                    return email;

                }

                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }

        public bool CheckAccessTokenAsBeareToken(string token)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(Cs))
            {
                SqlCommand cmd = new SqlCommand("Sp_Sales", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@Token", token);
                cmd.Parameters.AddWithValue("@Task", "verifytoken");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            if(dt!=null && dt.Rows[0]["result"].ToString() == "success")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}