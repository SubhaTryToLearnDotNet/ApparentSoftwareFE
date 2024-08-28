using Apparent.DBContext;
using Apparent.DBContext.Repositroy;
using Apparent.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Apparent.Controllers
{
    public class SignInController : Controller
    {
        private readonly CompanyService _companyService;
        public SignInController()
        {
            _companyService = new CompanyService();
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult signin(CompanyMaster master)
        {
            var Context = new CompanyDbContext();
            var table = Context.SignIn(master);
            if (table != null && table.Rows[0][0].ToString() != "Company email not exists")
            {
                if (table.Rows[0]["Type"].ToString() == "Seller" || table.Rows[0]["Type"].ToString() == "Both")
                {
                    Session["CompanyId"] = table.Rows[0]["CompanyId"].ToString();
                    //Session["CompanyName"] = table.Rows[0]["CompanyName"].ToString();
                    //Session["CompanyIcon"] = table.Rows[0]["CompanyIcon"].ToString();
                    //Session["Type"] = table.Rows[0]["Type"].ToString();
                    return RedirectToAction("Index", "Profile");
                }
                else
                {
                    Session["CompanyId"] = table.Rows[0]["CompanyId"].ToString();
                    //Session["FirstName"] = table.Rows[0]["FirstName"].ToString()+" " +table.Rows[0]["LastName"].ToString();

                    //Session["CompanyIcon"] = table.Rows[0]["CompanyIcon"].ToString();
                    //Session["Country_Code"] = table.Rows[0]["country"].ToString();
                    //Session["Type"] = table.Rows[0]["Type"].ToString();


                    return RedirectToAction("customerprof", "Profile");
                }
            }
            else
            {
                TempData["ErrorMsg"] = "Username  or password does not match";
                return RedirectToAction("Index");
            }

        }

        public async Task<ActionResult>email_check(EmailVerifyModel emailVerify)
        {
            var result = await _companyService.GetCompanyByEmail(emailVerify.Email);
            if (result != null)
            {
                var emailVerifyobj = new EmailVerifyModel
                {
                    Email = emailVerify.Email,
                    Timestamp = DateTime.UtcNow.Ticks
                };
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(emailVerifyobj);
                //byte[] encryptionKey = GenerateRandomKey(32);
                //string encryptedJson = EncryptData(json, encryptionKey);
                string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
                                            Request.ApplicationPath.TrimEnd('/') + "/";
                var hrflink = baseUrl + "ResetPassword/Index?encpid=" + emailVerify.Email ;
                emailVerifyobj.hrflink = hrflink;

                 EmailService emailService = new EmailService();
              await  emailService.Emailverify(emailVerifyobj);
                return Json(new { code = 200, msg = "Password reset link sent to email id." });
            }
            return Json(new { code = 404, msg = "Email is not registered !" });
        }
        public static byte[] GenerateRandomKey(int keySizeInBytes)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[keySizeInBytes];
                rng.GetBytes(key);
                return key;
            }
        }

        // Method to encrypt data using AES encryption
        public static string EncryptData(string data, byte[] key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = new byte[16]; // Use a random IV for each encryption

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] bytesToEncrypt = Encoding.UTF8.GetBytes(data);
                        csEncrypt.Write(bytesToEncrypt, 0, bytesToEncrypt.Length);
                        csEncrypt.FlushFinalBlock();
                    }
                    byte[] encryptedBytes = msEncrypt.ToArray();
                    return Convert.ToBase64String(encryptedBytes);
                }
            }

        }
    }
}