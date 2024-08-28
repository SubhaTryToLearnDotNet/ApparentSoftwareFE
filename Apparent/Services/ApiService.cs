using Apparent.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Configuration;
using System.Web.Http.Results;

namespace Apparent.Services
{
    public class ApiService : IApiService
    {

        public ApiService() { }
        private string Cs = ConfigurationManager.ConnectionStrings["abcd"].ConnectionString;
        public async Task<ProductSalseResponse> GetSalesDetails(CompayAccessRequestModel model)
        {
            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection con = new SqlConnection(Cs))
                {
                    SqlCommand cmd = new SqlCommand("Sp_Sales", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Id",model.company_id);
                    cmd.Parameters.AddWithValue("@Task", "GetSalesDetailsByToken");

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                };
                ProductSalseResponse response = new ProductSalseResponse();

                if (ds != null && ds.Tables.Count >= 2)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        response.status_code = 200;
                        response.status = "success";
                        response.company_id = Convert.ToInt32(ds.Tables[0].Rows[0]["CompanyId"]);
                        response.company_name = ds.Tables[0].Rows[0]["CompanyName"].ToString();
                        response.email = ds.Tables[0].Rows[0]["CompanyEmail"].ToString();
                    }
                    decimal Total_Amount = 0m;
                    decimal Total_Commission = 0m;
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        List<Product> productList = new List<Product>();
                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                        {
                            int j = 0;
                            DataRow productRow = ds.Tables[1].Rows[i];
                            int productId = Convert.ToInt32(productRow["ProductId"]);
                            string productName = productRow["ProductName"].ToString();
                            decimal commission = Convert.ToDecimal(productRow["Commission"].ToString());
                            decimal totalAmount = 0m;
                            Product product = new Product
                            {
                                product_id = productId,
                                product_name = productName,
                                product_commssion = commission,
                                data = new Data[0]
                               
                            };
                            List<Data> salesDataList = new List<Data>();
                            for ( j=i ; j < ds.Tables[1].Rows.Count; j++)
                            {
                                DataRow salesRow = ds.Tables[1].Rows[j];
                                if (Convert.ToInt32(salesRow["ProductId"]) == productId)
                                {
                                    decimal amount = salesRow["Amount"] != DBNull.Value ? Convert.ToDecimal(salesRow["Amount"]) : 0m;
                                    if (amount > 0)
                                    {
                                        totalAmount += amount;
                                    }
                                    Data salesData = new Data
                                    {
                                        tranasaction_id = Convert.ToInt32(salesRow["Transaction_Id"]),
                                        customer_name = salesRow["First_Name"].ToString() + " " + salesRow["Last_Name"].ToString(),
                                        amount = amount,
                                        plan = salesRow["Plan_Type"].ToString() != "0" ? salesRow["Plan_Type"].ToString() : "Free Trial",
                                        tenure = salesRow["Plan_Tenure"].ToString() != "0" ? salesRow["Plan_Tenure"].ToString() : "",
                                        tranasaction_date = salesRow["Settlement_date"].ToString()
                                    };

                                    salesDataList.Add(salesData);
                                }
                                else
                                {
                                     break;
                                }
                            }
                            i = j-1;
                            product.data = salesDataList.ToArray();
                            product.Sales = totalAmount;
                            Total_Commission += totalAmount * (commission / 100);
                            Total_Amount += totalAmount;
                            productList.Add(product);
                        }
                        response.total_amount = Total_Amount;
                        response.apparent_commission = Total_Commission;
                        response.product = productList.ToArray();
                    }
                    return response;
                }
                else
                {
                    response.error = "Company Not Found";
                    response.status_code = 404;
                    return response;
                }

            }
            catch
            {
                return null;
            }


        }

        public CompayAccessRequestModel AuthenticateUser(CompayAccessRequestModel model)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Cs))
                {
                    SqlCommand cmd = new SqlCommand("SP_ApiOAuthCheck", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Id", model.company_id);
                    cmd.Parameters.AddWithValue("@Token", model.company_token);
                    cmd.Parameters.AddWithValue("@Task", "IsAuthenticate");
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);
                };
                if(dt!=null &&  dt.Rows.Count>0)
                {
                    DataRow productRow = dt.Rows[0];
                    var User = new CompayAccessRequestModel
                    {
                        company_id = Convert.ToInt32(productRow["CompanyId"].ToString()),
                        company_token = productRow["Token"].ToString()
                    };
                    return User;
                }
                else
                {
                    return null;
                }
            }
            catch(Exception ex)
            {
                return null ;
            }
        }
    }
}
