using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;
using Apparent.Model;
using Mandrill;
using Mandrill.Models;
using Mandrill.Requests.Messages;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.UI.WebControls;

namespace Apparent
{
    public class EmailService
    {
        private readonly string _SenderEmail;
        private readonly string _SenderPassword;
        private readonly int _Port;
        private readonly string _Host;
        private readonly bool _EnableSsl;
        private readonly string _ApiKey;
        public EmailService() {
            _SenderEmail = ConfigurationManager.AppSettings.Get("email");
            _ApiKey = ConfigurationManager.AppSettings.Get("ApiKey");
            _SenderPassword = ConfigurationManager.AppSettings.Get("password");
            _Port = Convert.ToInt32(ConfigurationManager.AppSettings.Get("Port"));
            _Host = ConfigurationManager.AppSettings.Get("Host");
            _EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EnableSsl"));


        }

        public async Task Emailverification(string email,string verificationCode)
        {

            try
            { // start email


                MandrillApi mandrillApi = new MandrillApi(_ApiKey);

                List<EmailAddress> toEmail = new List<EmailAddress>()
            {
                new EmailAddress(email)
            };

                //MailMessage mail = new MailMessage(_SenderEmail, email);
                //mail.Subject = "Email verification";

                StringBuilder mailBodyHtml = new StringBuilder();
                mailBodyHtml.Append(System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Templates/Emailverification.html")));
                mailBodyHtml.Replace("#Password#", verificationCode);
                EmailMessage emailMessage = new EmailMessage()
                {
                    FromEmail = _SenderEmail,
                    FromName = "Apparent",
                    To = toEmail,
                    Subject = "Email verification",
                    Html = mailBodyHtml.ToString()
                };

                List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));

                //mail.Body = mailBodyHtml.ToString();
                //mail.IsBodyHtml = true;
                //SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                //smtp.Port = _Port;
                //smtp.EnableSsl = _EnableSsl;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new NetworkCredential(_SenderEmail,_SenderPassword);
                //smtp.Send(mail);
                // end email
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        public async Task Emailverify(EmailVerifyModel model)
        {

            try
            { // start email


                MandrillApi mandrillApi = new MandrillApi(_ApiKey);

                List<EmailAddress> toEmail = new List<EmailAddress>()
                {
                new EmailAddress(model.Email)
            };

                //MailMessage mail = new MailMessage(_SenderEmail, model.Email);
                //mail.Subject = "Email verification";
                StringBuilder mailBodyHtml = new StringBuilder();
                mailBodyHtml.Append(System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Templates/ForgotPassword.html")));
                mailBodyHtml.Replace("#LINK#", model.hrflink);


                EmailMessage emailMessage = new EmailMessage()
                {
                    FromEmail = _SenderEmail,
                    FromName = "Apparent",
                    To = toEmail,
                    Subject = "Password Recovery - Apparent",
                    Html = mailBodyHtml.ToString()
                };

                List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));

                //mail.Body = mailBodyHtml.ToString();
                //mail.IsBodyHtml = true;
                //SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                //smtp.Port = _Port;
                //smtp.EnableSsl = _EnableSsl;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new NetworkCredential(_SenderEmail, _SenderPassword);
                //smtp.Send(mail);
                // end email
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public async Task Payment_Recipt_Email(Response response,string email,string user_name)
        {

            try
            { // start email

                DateTime transactionDate = (DateTime)response.transaction_date; // Assuming response.transaction_date is of type DateTime
                string formattedDate = transactionDate.ToString("dd MMM yyyy");


                MandrillApi mandrillApi = new MandrillApi(_ApiKey);

                List<EmailAddress> toEmail = new List<EmailAddress>()
            {
                new EmailAddress(email)
            };

                StringBuilder mailBodyHtml = new StringBuilder();
                mailBodyHtml.Append(System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Templates/payment_recipt_emial.html")));
                mailBodyHtml.Replace("#PAYMENTID#", response.transaction_id);
                mailBodyHtml.Replace("#NAME#", user_name);
                mailBodyHtml.Replace("#CARDTYPE#", response.card_category);
                mailBodyHtml.Replace("#PAYMENTDATE#", formattedDate);
                mailBodyHtml.Replace("#PRICE#", response.decimal_amount.ToString());

                EmailMessage emailMessage = new EmailMessage()
                {
                    FromEmail = _SenderEmail,
                    FromName = "Apparent",
                    To = toEmail,
                    Subject = "Payment processing",
                    Html = mailBodyHtml.ToString()
                };

                List<EmailResult> emailResults = await mandrillApi.SendMessage(new SendMessageRequest(emailMessage));





                //MailMessage mail = new MailMessage(_SenderEmail, email);
                //mail.Subject = "Email verification";
                //StringBuilder mailBodyHtml = new StringBuilder();
                //mailBodyHtml.Append(System.IO.File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/Content/Templates/payment_recipt_emial.html")));
                //mailBodyHtml.Replace("#PAYMENTID#", response.transaction_id);
                //mailBodyHtml.Replace("#NAME#", user_name);
                //mailBodyHtml.Replace("#CARDTYPE#", response.card_category);
                //mailBodyHtml.Replace("#PAYMENTDATE#", formattedDate);
                //mailBodyHtml.Replace("#PRICE#", response.decimal_amount.ToString());



                //mail.Body = mailBodyHtml.ToString();
                //mail.IsBodyHtml = true;
                //SmtpClient smtp = new SmtpClient("smtp.gmail.com");
                //smtp.Port = _Port;
                //smtp.EnableSsl = _EnableSsl;
                //smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new NetworkCredential(_SenderEmail, _SenderPassword);
                //smtp.Send(mail);
                // end email
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}