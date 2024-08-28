using Apparent.Model;
using Apparent.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;

namespace Apparent.Repository
{
    public class ApiPaymentService : IApiPaymentService
    {

        private string Cs = ConfigurationManager.ConnectionStrings["abcd"].ConnectionString;
        public string CreatePaymentRequest(PaymentRequestModel model)
        {
            try
            {
                DataTable dt = new DataTable();
                string PaymentKey = null;
                string RequestKey = GeneratePaymentRequestKey();
                using (SqlConnection con = new SqlConnection(Cs))
                {
                    SqlCommand cmd = new SqlCommand("Sp_PaymentRequest", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@Price", model.Price);
                    cmd.Parameters.AddWithValue("@RedirectUrl", model.RedirectUrl);
                    cmd.Parameters.AddWithValue("@RequestKey", RequestKey);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@Task", "CreatePaymentRequest");
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                }
                if (dt != null && dt.Rows.Count>0)
                {
                    DataRow dr = dt.Rows[0];
                    PaymentKey = dr["PaymentRequestKey"].ToString();
                    return PaymentKey;
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

       public Respons CheckRequestKey(string Key)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(Cs))
            {
                SqlCommand cmd = new SqlCommand("Sp_PaymentRequest", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@RequestKey", Key);
                cmd.Parameters.AddWithValue("@Task", "CheckRequestKey");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

            }
            Respons respons = new Respons();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                respons.Price=Convert.ToDecimal(dr["Price"].ToString());
                respons.RedirectUrl = dr["RedirectUrl"].ToString();
                respons.PaymentRequestKey = dr["PaymentRequestKey"].ToString();
                respons.Email = dr["Email"].ToString();
                return respons;
            }
            else
            {
                return respons;
            }
        }

       public  RedirectRespons AddApiPaymentRespons(RedirectRespons model)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(Cs))
            {
                SqlCommand cmd = new SqlCommand("SP_ApiPayment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@Amount", model.amount);
                cmd.Parameters.AddWithValue("@Currency", model.currency);
                cmd.Parameters.AddWithValue("@Email", model.email);
                cmd.Parameters.AddWithValue("@Settlement_date", model.settlement_date);
                cmd.Parameters.AddWithValue("@Transaction_date", model.transaction_date);
                cmd.Parameters.AddWithValue("@PaymentType", model.payment_type);
                cmd.Parameters.AddWithValue("@Transaction_id", model.transaction_id);
                cmd.Parameters.AddWithValue("@User_name", model.user_name);
                cmd.Parameters.AddWithValue("@Task", "AddPaymentRespons");
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);

            }
            RedirectRespons respons = new RedirectRespons();
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                respons.amount = Convert.ToDecimal(dr["Amount"].ToString());
                respons.user_name = dr["User_Name"].ToString();
                //respons.transaction_date = Convert.ToDateTime( dr["Transaction_date"].ToString());
                respons.email = dr["Email"].ToString();
                respons.currency = dr["Currency"].ToString();
                respons.payment_type = dr["PaymentType"].ToString();
                respons.transaction_id = dr["Transaction_id"].ToString();
                respons.settlement_date = dr["settlement_date"].ToString();
                return respons;
            }
            else
            {
                return respons;
            }
        }

        public RedirectRespons AddAppPayRespons(RedirectRespons model)
        {
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection con = new SqlConnection(Cs))
                {
                    SqlCommand cmd = new SqlCommand("SP_APPARENT_APY", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Amount", model.amount);
                    cmd.Parameters.AddWithValue("@Currency", model.currency);
                    cmd.Parameters.AddWithValue("@Email", model.email);
                    cmd.Parameters.AddWithValue("@Transaction_Date", model.transaction_date);
                    cmd.Parameters.AddWithValue("@PaymentType", model.payment_type);
                    cmd.Parameters.AddWithValue("@Transaction_Id", model.transaction_id);
                    cmd.Parameters.AddWithValue("@User_name", model.user_name);
                    cmd.Parameters.AddWithValue("@Task", "ADD_APP_PAY_RESPONS");
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(dt);

                }
                RedirectRespons respons = new RedirectRespons();
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    respons.amount = Convert.ToDecimal(dr["Amount"].ToString());
                    respons.user_name = dr["User_Name"].ToString();
                    respons.email = dr["Email"].ToString();
                    respons.currency = dr["Currency"].ToString();
                    respons.payment_type = dr["PaymentType"].ToString();
                    respons.transaction_id = dr["Transaction_id"].ToString();
                    return respons;
                }
                else
                {
                    return respons;
                }
            }
               catch {
                return null;
            }

        }


        public string GeneratePaymentRequestKey()
        {
            Guid guid = Guid.NewGuid();
            long ticks = DateTime.Now.Ticks;
            return $"{guid}_{ticks}";
        }
    }
}