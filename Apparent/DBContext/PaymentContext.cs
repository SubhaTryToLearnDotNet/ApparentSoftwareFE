using Apparent.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Azure;
using System.Reflection;
using System.Timers;
using System.ComponentModel.Design;

namespace Apparent.DBContext
{
    public class PaymentContext
    {
        public PaymentContext() { }

        private string Cs = ConfigurationManager.ConnectionStrings["defaultstring"].ConnectionString;

        private string Cs1 = ConfigurationManager.ConnectionStrings["abcd"].ConnectionString;


        public string SubmitSubcriptionDetails(ViewModel model)
        {
            try
            {
                string IdentityKey = null;
                DataTable dt1 = new DataTable();
                SqlConnection con = new SqlConnection(Cs1);
                SqlCommand cmd = new SqlCommand("Sp_SubscriptionDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@UserId", model.company.CompanyId);
                cmd.Parameters.AddWithValue("@FirstName", model.company.First_Name);
                cmd.Parameters.AddWithValue("@LastName", model.company.Last_Name);
                cmd.Parameters.AddWithValue("@Email", model.company.CompanyEmail);
                cmd.Parameters.AddWithValue("@ProductId", model.payment_master.ProductId);
                cmd.Parameters.AddWithValue("@PlanID", model.payment_master.PlanId);
                cmd.Parameters.AddWithValue("@PlanName", model.payment_master.Plan_Name);

                cmd.Parameters.AddWithValue("@Task", "CreateSubscriptionEntry");

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt1);
                con.Close();

                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    DataRow dr = dt1.Rows[0];
                    IdentityKey = dr["IdentityKey"].ToString();
                    return IdentityKey;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<SubscriptionRespons> GetSubscriptionPaymentRespons(SubscriptionRespons subscription)
        {
            try
            {
                DataTable  dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs1);
                SqlCommand cmd = new SqlCommand("Sp_PaymentRespons", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@Payment_Type", subscription.Payment_Type);
                cmd.Parameters.AddWithValue("@Plan_Tenure", subscription.Plan_Tenure);
                cmd.Parameters.AddWithValue("@Plan_Type", subscription.Plan_Type);
                cmd.Parameters.AddWithValue("@ProductId", subscription.ProductId);
                cmd.Parameters.AddWithValue("@CompanyId",subscription.CompanyId);
                cmd.Parameters.AddWithValue("@transaction_id", subscription.Transaction_id);
                cmd.Parameters.AddWithValue("@settlement_date", subscription.Settlement_date);
                cmd.Parameters.AddWithValue("@decimal_amount", subscription.Amount);
                cmd.Parameters.AddWithValue("@token", subscription.token);
                cmd.Parameters.AddWithValue("@Subscription_Expired", subscription.Subscription_Expired);
                cmd.Parameters.AddWithValue("@Task", "submitsubscription_respons");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                SubscriptionRespons respons = new SubscriptionRespons();
                respons.CompanyId = dt.Rows[0]["CompanyId"].ToString();
                respons.ProductId = dt.Rows[0]["ProductId"].ToString();
                respons.Settlement_date  =Convert.ToDateTime( dt.Rows[0]["Settlement_date"].ToString());
                respons.Transaction_id = dt.Rows[0]["Transaction_id"].ToString();
                respons.Amount = Convert.ToInt32(dt.Rows[0]["Amount"].ToString());
                respons.Subscription_Expired = Convert.ToDateTime( dt.Rows[0]["Subscription_Expired"].ToString());

                respons.Payment_Type = dt.Rows[0]["Payment_Type"].ToString();
                respons.Plan_Tenure = dt.Rows[0]["Plan_Tenure"].ToString();

                return respons;
            }
            catch (Exception ex)
            {
                return  null;

            }
        }

        public async Task<PaymentMaster> GetPaymentSystem (string country_code)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("Sp_Payment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@Country_Code", country_code);
                cmd.Parameters.AddWithValue("@Task", "GetPaymentSystem");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(ds);
                con.Close();

                PaymentMaster paymentMaster = new PaymentMaster();


                ;


                
                    if (ds.Tables[0].Rows.Count > 0) {

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        paymentMaster.InternationalPayment.Add(
                        dr["International_payment_type"].ToString()

                        );
                    }                   
                        }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        paymentMaster.CryptoPayment.Add(
                        dr["crypto_payment_type"].ToString()
                        );
                    }
                }
                if (ds.Tables[2].Rows.Count > 0)
                {

                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        paymentMaster.CountryWisePayment.Add( new CountryWisePayment
                        {
                            Payment_icon = dr["payment_icon"].ToString(),
                            Payment_Name = dr["countrywise_payment_type"].ToString(),
                            Country_code = dr["country_code"].ToString()
                        }
                       
                        );
                    }
                }

                return paymentMaster;

            }
            catch
            {
                return null;
            }
        }


        public async Task<DataTable> Submit_CardPayment_Response(Model.Response model, string CompanyId, string ProductId, string Plan_Type, string Plan_Tenure)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs1);
                SqlCommand cmd = new SqlCommand("Sp_PaymentRespons", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@authorization", model.authorization);
                cmd.Parameters.AddWithValue("@card_token", model.card_token);
                cmd.Parameters.AddWithValue("@decimal_amount", model.decimal_amount);
                cmd.Parameters.AddWithValue("@transaction_date", model.transaction_date);
                cmd.Parameters.AddWithValue("@successful", model.successful);
                cmd.Parameters.AddWithValue("@settlement_date", model.settlement_date);
                cmd.Parameters.AddWithValue("@transaction_id", model.transaction_id);
                cmd.Parameters.AddWithValue("@rrn", model.rrn);
                cmd.Parameters.AddWithValue("@message", model.message);
                cmd.Parameters.AddWithValue("@reference", model.reference);
                //cmd.Parameters.AddWithValue("@id", model.id);
                cmd.Parameters.AddWithValue("@currency", model.currency);
                cmd.Parameters.AddWithValue("@Plan_Tenure", Plan_Tenure);
                cmd.Parameters.AddWithValue("@card_number", model.card_number);
                cmd.Parameters.AddWithValue("@card_type", model.card_type);
                cmd.Parameters.AddWithValue("@ProductId", ProductId);
                cmd.Parameters.AddWithValue("@Plan_Type", Plan_Type);
                cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
                cmd.Parameters.AddWithValue("@Task", "CardPaymentRespons");
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

        public  async Task<PaymentMaster> GetProductPricebyPlanType(string ProductId, string Plan_Type, string Plan_Tenure)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs1);
                SqlCommand cmd = new SqlCommand("Sp_PaymentRespons", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@ProductId", ProductId);
                cmd.Parameters.AddWithValue("@Plan_Type", Plan_Type);
                cmd.Parameters.AddWithValue("@Plan_Tenure", Plan_Tenure);
                cmd.Parameters.AddWithValue("@Task","GetProductPrice"); 
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
                con.Close();
                PaymentMaster paymentMaster = new PaymentMaster();
                paymentMaster.Price = dt.Rows[0]["Price"].ToString();
                paymentMaster.Plan_tenure = dt.Rows[0]["Plan_tenure"].ToString();
                paymentMaster.Plan_Name = dt.Rows[0]["Plan_Name"].ToString();
                paymentMaster.ProductId = dt.Rows[0]["ProductId"].ToString();
                return paymentMaster;

            }
            catch
            {
                return null;
            }
        }

       public async Task<bool> Sold_FreeToken(string CompanyId,string ProductId,DateTime Subscription_Expired, DateTime settlement_date,string PlanId)
        {
           
            SqlConnection con = new SqlConnection(Cs1);
            SqlCommand cmd = new SqlCommand("Sp_PaymentRespons", con);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
			cmd.Parameters.AddWithValue("@PlanId", PlanId);
			cmd.Parameters.AddWithValue("@CompanyId", CompanyId);
            cmd.Parameters.AddWithValue("@Subscription_Expired", Subscription_Expired);
            cmd.Parameters.AddWithValue("@settlement_date", settlement_date);
            //cmd.Parameters.AddWithValue("@token", token);
            cmd.Parameters.AddWithValue("@Task", "Sold_freetoken");
            int a = cmd.ExecuteNonQuery();
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

    }
}