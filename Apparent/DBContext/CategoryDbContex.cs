using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Apparent.Model;

namespace Apparent.DBContext
{
    public class CategoryDbContex
    {
        public CategoryDbContex() { }

        private string Cs = ConfigurationManager.ConnectionStrings["abcd"].ConnectionString;



        public async Task<bool> add_category(string category,string companyid)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Category", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyId", companyid);
                cmd.Parameters.AddWithValue("@category", category);
                cmd.Parameters.AddWithValue("@Task", "add_category");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                if (dt.Rows[0]["result"].ToString() == "success")
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

        public async Task<List<Category>> select_category(string companyid)
        {
            try
            {           
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Category", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@CompanyId", companyid);
                cmd.Parameters.AddWithValue("@Task", "select_categoryBycompany");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                List<Category> categories = new List<Category>();
                foreach (DataRow dr in dt.Rows)
                {
                    Category category = new Category();
                    category.category_id = (long)dr["categoryid"];
                    category.category_name = dr["category_name"].ToString();
                    category.status = dr["status"].ToString();
                    category.companyid = dr["companyid"].ToString();
                    category.created_date = dr["created_date"].ToString();
                    category.cat_suggestion = dr["suggestion"].ToString();
                    category.suggestion_IsActive = dr["suggestion_IsActive"].ToString();
                    categories.Add(category);
                }

                return categories;

            }
            catch
            {
                return null;
            }



        }
        public async Task<bool>update_category(Category category)
        {
            try
            {
               
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Category", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@categoryid", category.category_id);
                cmd.Parameters.AddWithValue("@category", category.category_name);
                cmd.Parameters.AddWithValue("@Task", "update_category");
               int a = cmd.ExecuteNonQuery();
                con.Close();
                con.Close();
                if (a>0)
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
    }
}