using Apparent.DBContext.EntityDbContext;
using Apparent.Model;
using Apparent.Model.EntityTBL;
using Azure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace Apparent.DBContext.Repositroy
{
    public class PaymentService
    {
        private readonly AppDbContext _appDbContext;
        private readonly string _PaymentTokenHeaderauthorization;

        public PaymentService()
        {
            _appDbContext = new AppDbContext();
            _PaymentTokenHeaderauthorization = ConfigurationManager.AppSettings.Get("PaymentTokenHeaderauthorization");
        }


        public async Task<CardMasterModel> PaymentSubmit(CardMasterModel model)
        {
            try
            {
                DateTime currentDateTime = DateTime.Now;
             
                string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                var ip = "";
                string hostName = Dns.GetHostName();
               
                IPAddress[] ipAddresses = Dns.GetHostAddresses(hostName);
                
                foreach (IPAddress ipAddress in ipAddresses)
                {
                    ip = ipAddress.ToString();
                }
                var payment = new CardPaymentMaster
                {

                card_expiry = model.month + "/" + model.year,
                 card_number = model.CardNumber,
                card_holder = model.CardholderName,
                cvv = model.CVV,
                    customer_ip = ip,
                amount = Convert.ToDecimal(Convert.ToInt32(model.Amount) * 100),
                reference = formattedDateTime + "/" + model.CardNumber,
                currency = "AUD",
                };

               var respons =  await purchase(payment);

               if( respons.message == "Approved")
                {
                    model.successful = true;
                    model.status = respons.message;
                    model.tetransaction_id = respons.transaction_id;
                    var submit_respons = new TBL_APPARENT_SERVICE_PAYMENT
                    {
                        UserName = model.User_Name,
                        card_holder = respons.card_holder,
                        card_number = respons.card_number,
                        card_type = respons.card_type,
                        currency = respons.currency,
                        decimal_amount = respons.decimal_amount,
                        email = model.Email,
                        Notes = model.Notes,
                        reference = respons.reference,
                        respons_id = respons.id,
                        settlement_date = respons.settlement_date,
                        successful = respons.successful,
                        message = respons.message,
                        transaction_date = respons.transaction_date,
                        transaction_id = respons.transaction_id,
                        card_category = respons.card_category,
                    };
                    _appDbContext.TBL_APPARENT_SERVICE_PAYMENT.Add(submit_respons);
                     await _appDbContext.SaveChangesAsync();
                    EmailService emailService = new EmailService();
                    await emailService.Payment_Recipt_Email(respons,model.Email,model.User_Name);
                }
                else
                {
                    model.status = "Declined";
                    model.successful = false;
                }
                model.message = respons.message;
               
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           

        }
        

        public async Task<Model.Response> purchase(CardPaymentMaster cardPayment )
        {
            try
            {
                string apiUrl = ConfigurationManager.AppSettings["FatzebraURL"].ToString();
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Add("accept", "application/json");
                    httpClient.DefaultRequestHeaders.Add("authorization", _PaymentTokenHeaderauthorization);

                    string data = JsonConvert.SerializeObject(cardPayment);

                    StringContent content4 = new StringContent(data, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content4);

                    Model.Response response1 = new Model.Response();
                    if (response.IsSuccessStatusCode)
                    {
                        string PaymentContent = await response.Content.ReadAsStringAsync();
                        CardPaymentRespons paymentRespons = JsonConvert.DeserializeObject<CardPaymentRespons>(PaymentContent);
                        if (paymentRespons.successful == true && paymentRespons.response.message == "Approved")
                        {
                           
                            response1.authorization = paymentRespons.response.authorization;
                            response1.id = paymentRespons.response.id;
                            response1.decimal_amount = paymentRespons.response.decimal_amount;
                            response1.card_token = paymentRespons.response.card_token;
                            response1.card_type = paymentRespons.response.card_type;
                            response1.reference = paymentRespons.response.reference;
                            response1.card_number = paymentRespons.response.card_number;
                            response1.transaction_date = Convert.ToDateTime(paymentRespons.response.transaction_date);
                            response1.settlement_date = paymentRespons.response.settlement_date;
                            response1.transaction_id = paymentRespons.response.transaction_id;
                            response1.currency = paymentRespons.response.currency;
                            response1.message = paymentRespons.response.message;
                            response1.card_category = paymentRespons.response.card_category;
                            response1.rrn = paymentRespons.response.rrn;
                            response1.successful = paymentRespons.response.successful;
                            return response1;
                        }
                        else
                        {
                            if (paymentRespons.errors != null && paymentRespons.errors.Length > 0 && paymentRespons.errors[0] != null)
                            {

                                response1.message = paymentRespons.errors[0].ToString();
                                return response1;
                            }
                            else{
                                response1.message = paymentRespons.response.message.ToString();
                                return response1;
                            }
                        }
                    }
                    else
                    {

                        
                        return null;
                    }


                }
            }
            catch (Exception e)
            {
                return null;
            }
        }


        public async Task<InvoiceModel> Get_invoice(string transaction_id)
        {
            try
            {
                if (!string.IsNullOrEmpty(transaction_id))
                {
                    
                    var Respons = await _appDbContext.TBL_APPARENT_SERVICE_PAYMENT
               .Where(x => x.transaction_id == transaction_id)
               .FirstOrDefaultAsync();

                    if (Respons != null)
                    {
                        DateTime transactionDate = (DateTime)Respons.transaction_date;
                        string formattedDate = transactionDate.ToString("dd MMM yyyy");
                        var respons = new InvoiceModel
                        {
                            UserName = Respons.UserName,
                            settlement_date = formattedDate,
                            transaction_id = Respons.transaction_id,
                            decimal_amount = Respons.decimal_amount,
                            card_category = Respons.card_category,


                        };
                        return respons;
                    }
                    else
                    {
                        return  null ;
                    }
                }
                else
                {
                    return null;
                }
            }    
            catch (Exception e) { return null; }
        }

        
    }
}