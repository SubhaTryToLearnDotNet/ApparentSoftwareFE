using Apparent.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Apparent.DBContext
{
    public class SellerMasterDbContex
    {

        private string Cs = ConfigurationManager.ConnectionStrings["abcd"].ConnectionString;


        public DataTable SellerDeatils(string UserId)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("SP_SellerMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();

                cmd.Parameters.AddWithValue("@SellerId", UserId);
                cmd.Parameters.AddWithValue("@Task", "SelectSeller");

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
        public DataTable UpdateSellerDeatils(SellerMaster master)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("SP_SellerMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@FirstName", master.FirstName);
                cmd.Parameters.AddWithValue("@LastName", master.LastName);
                cmd.Parameters.AddWithValue("@Email", master.Email);
                cmd.Parameters.AddWithValue("@ImagePath", master.Imagepath);
                cmd.Parameters.AddWithValue("@Password", master.Password);
                cmd.Parameters.AddWithValue("@About", master.About);
                cmd.Parameters.AddWithValue("@SellerId", master.SellerId);
                cmd.Parameters.AddWithValue("@CurrentPassword", master.CurrentPassword);
                cmd.Parameters.AddWithValue("@Task", "UpdateSeller");
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

        public DataTable RegistrationSubmit(SellerMaster master)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("SP_SellerMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@FirstName", master.FirstName);
                cmd.Parameters.AddWithValue("@LastName", master.LastName);
                cmd.Parameters.AddWithValue("@Email", master.Email);
                cmd.Parameters.AddWithValue("@ImagePath", master.Imagepath);
                cmd.Parameters.AddWithValue("@Password", master.Password);
                cmd.Parameters.AddWithValue("@Contact", master.Contact);
                cmd.Parameters.AddWithValue("@About", master.About);
                cmd.Parameters.AddWithValue("@CountryId",master.Id);
                cmd.Parameters.AddWithValue("@Task", "SellerSignUp");
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
       

        public DataTable SellerList()
        {
            try
            {

                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("SP_SellerMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();




                cmd.Parameters.AddWithValue("@Task", "SelectSellerlist");
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
        public DataTable CountryList()
        {
            try
            {
                DataTable dt = new DataTable();
                SqlConnection con = new SqlConnection(Cs);
                SqlCommand cmd = new SqlCommand("SP_SellerMaster", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                cmd.Parameters.AddWithValue("@Task", "SelectCountryList");
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
    }
}