using Apparent.DBContext;
using Apparent.DBContext.Repositroy;
using Apparent.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Apparent.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly CompanyService _companyService;
         public ProfileController() {
            _companyService = new CompanyService();
        }

        // GET: Profile
        public async Task<ActionResult> Index()
        {
            if (Session["CompanyId"] != null && Session["Type"].ToString() == "Seller")
            {
                string id = Session["CompanyId"].ToString();
                CompanyDbContext company = new CompanyDbContext();
                ViewModel model = new ViewModel();
                model.company = await company.GetCompanyDetails(id);
                model.company.ActionName = "Profile";
                return View(model);

            }
            else
            {
                return RedirectToAction("Index", "SignIn");
            }

        }

        public async Task<ActionResult> CompanyToken()
        {
            if (Session["CompanyId"] != null && Session["Type"].ToString() == "Seller")
            {
                string id = Session["CompanyId"].ToString();
                CompanyDbContext company = new CompanyDbContext();
                ViewModel model = new ViewModel();
                model.company = await company.GetCompanyDetails(id);
                model.company.ActionName = "CompanyToken";
                return View(model);

            }
            else
            {
                return RedirectToAction("Index", "SignIn");
            }

        }

        [HttpPost]
        public async Task<ActionResult> SaveImage(CompanyMaster obj)
        {
            try
            {


                obj.CompanyId = Session["CompanyId"].ToString();


                StorageService storageService = new StorageService();
                CompanyDbContext company = new CompanyDbContext();

                foreach (string file in Request.Files)
                {

                    if (file != null && file.Contains("CompanyImage"))
                    {
                        var filecontent = Request.Files[file];
                        var Icon = filecontent.FileName;


                        Guid fileGuid = Guid.NewGuid();
                        var FilePath = fileGuid.ToString("N").Substring(0, 12) + "." + filecontent.FileName.Split('.')[1];



                        //filecontent.SaveAs(Server.MapPath(Path.Combine("~/Content/Web/ProductLogo", FilePath)));
                        Stream imageStream = filecontent.InputStream;
                        string containerName = "companyicon" + obj.CompanyId;
                        BlobResult imageResult = await storageService.UploadFile(imageStream, containerName, FilePath);
                        obj.CompanyIcon = imageResult.Uri;

                    }

                }

                bool update = await company.uploadIcon(obj);
                if (update)
                {
                    return Json(new { code = 200 });

                }
                else
                {
                    return Json(new { code = 400 });
                }
            }
            catch
            {
                return Json(new { code = 500 });
            }
        }



        [HttpPost]
        public async Task<ActionResult> update(CompanyMaster obj)
        {
            if (Session["CompanyId"] != null)
            {
                var Companyid = Session["CompanyId"].ToString();
                CompanyDbContext company = new CompanyDbContext();
                bool update = await company.UpdateDetails(obj, Companyid);
                if (update)
                {
                    return Json(new { code = 200 });

                }
                else
                {
                    return Json(new { code = 400 });
                }
            }
            else
            {
                return Json(new { code = 500, url = "/SignIn" });
            }

        }

        public async Task<ActionResult> customerprof()
        {
            if (Session["CompanyId"] != null && Session["Type"].ToString() == "Customer")
            {
                string id = Session["CompanyId"].ToString();
                CompanyDbContext company = new CompanyDbContext();
                ViewModel model = new ViewModel();

                model.company = await company.GetCompanyDetails(id);
                model.company.ActionName = "customerprof";
                Session["Country_Code"] = model.company.Country;
                string IpAddress = await GetLocalIPAddressAsync();
                using (HttpClient client = new HttpClient())
                {
                    string address = $"https://ipinfo.io/{IpAddress}?token=f3c40b99bd4a2d";
                    var result = await client.GetStringAsync(address);
                    IpInfo ipInfo = JsonConvert.DeserializeObject<IpInfo>(result);
                    string cy = ConvertCountry(ipInfo.country);
                    model.company.Country = cy;
                    Session["Country_Code"] = cy;
                }
                return View(model);

            }
            else
            {
                return RedirectToAction("Index", "SignIn");
            }

        }
        public async Task<string> GetLocalIPAddressAsync()
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Use the httpbin service to get your public IP address
                    string apiUrl = "https://httpbin.org/ip";

                    // Make an HTTP GET request to the service
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    // Check if the request was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Read and parse the JSON response
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        // Assuming the response is in JSON format, you may need to deserialize it
                        // using a JSON library like Newtonsoft.Json
                        // For simplicity, we'll just extract the IP address string manually
                        int startIndex = jsonResponse.IndexOf("origin") + 10;
                        int endIndex = jsonResponse.IndexOf("}", startIndex)-2;
                        string publicIp = jsonResponse.Substring(startIndex, endIndex - startIndex);

                        return publicIp;
                    }
                    else
                    {
                        // Handle the case where the request was not successful
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }


                // Get all network interfaces on the machine
                //NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

                //foreach (NetworkInterface networkInterface in networkInterfaces)
                //{
                //    // Check if the network interface is operational and not a loopback or tunnel interface
                //    if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                //        networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                //        networkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                //    {
                //        // Get the IP properties of the network interface
                //        IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();

                //        // Get the first unicast address (IPv4) from the network interface
                //        UnicastIPAddressInformationCollection unicastAddresses = ipProperties.UnicastAddresses;
                //        foreach (UnicastIPAddressInformation address in unicastAddresses)
                //        {
                //            if (address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                //            {
                //                return address.Address.ToString();
                //            }
                //        }
                //    }
                //}

                return null; // No suitable IP address found
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<ActionResult> customer_profileupdate(CompanyMaster obj)
        {
            if (Session["CompanyId"] != null)
            {
                var Companyid = Session["CompanyId"].ToString();
                CompanyDbContext company = new CompanyDbContext();
                bool update = await company.customerprofiledetailupdate(obj, Companyid);
                if (update)
                {
                    return Json(new { code = 200 });

                }
                else
                {
                    return Json(new { code = 400 });
                }
            }
            else
            {
                return Json(new { code = 500, url = "/SignIn" });
            }

        }
        public async Task<ActionResult> ResetPassword(CompanyMaster obj)
        {
            if (Session["CompanyId"] != null)
            {
                var Companyid = Session["CompanyId"].ToString();
                CompanyDbContext company = new CompanyDbContext();
                bool update = await company.password_reset(obj, Companyid);
                if (update)
                {
                    var url = "/SignIn";
                    return Json(new { code = 200, url });

                }
                else
                {
                    return Json(new { code = 400 });
                }
            }
            else
            {
                return Json(new { code = 500, url = "/SignIn" });
            }

        }

        public async Task<ActionResult> my_product()
        {
            if (Session["CompanyId"] != null && Session["Type"].ToString() == "Customer")
            {
                string id = Session["CompanyId"].ToString();
                ProductDBContext product = new ProductDBContext();
                ViewModel model = new ViewModel();

                model.Products.AddRange(await product.GetProduct(id));
                model.company = await _companyService.GetCompanyById(id);
                model.company.ActionName = "my_product";

                return View(model);

            }
            else
            {
                return RedirectToAction("Index", "SignIn");
            }

        }

        public string ConvertCountry(string Country)
        {
            Dictionary<string, string> map = new Dictionary<string, string>
            {
                {"AF","AF/AFG"},
                {"AL","AL/ALB"},
                {"DZ","DZ/DZA"},
                {"AS","AS/ASM"},
                {"AD","AD/AND"},
                {"AO","AO/AGO"},
                {"AI","AI/AIA"},
                {"AQ","AQ/ATA"},
                {"AG","AG/ATG"},
                {"AR","AR/ARG"},
                {"AM","AM/ARM"},
                {"AW","AW/ABW"},
                {"AU","AU/AUS"},
                {"AT","AT/AUT"},
                {"AZ","AZ/AZE"},
                {"BS","BS/BHS"},
                {"BH","BH/BHR"},
                {"BD","BD/BGD"},
                {"BB","BB/BRB"},
                {"BY","BY/BLR"},
                {"BE","BE/BEL"},
                {"BZ","BZ/BLZ"},
                {"BJ","BJ/BEN"},
                {"BM","BM/BMU"},
                {"BT","BT/BTN"},
                {"BO","BO/BOL"},
                {"BQ","BQ/BES"},
                {"BA","BA/BIH"},
                {"BW","BW/BWA"},
                {"BV","BV/BVT"},
                {"BR","BR/BRA"},
                {"IO","IO/IOT"},
                {"BN","BN/BRN"},
                {"BG","BG/BGR"},
                {"BF","BF/BFA"},
                {"BI","BI/BDI"},
                {"CV","CV/CPV"},
                {"KH","KH/KHM"},
                {"CM","CM/CMR"},
                {"CA","CA/CAN"},
                {"KY","KY/CYM"},
                {"CF","CF/CAF"},
                {"TD","TD/TCD"},
                {"CL","CL/CHL"},
                {"CN","CN/CHN"},
                {"CX","CX/CXR"},
                {"CC","CC/CCK"},
                {"CO","CO/COL"},
                {"KM","KM/COM"},
                {"CD","CD/COD"},
                {"CG","CG/COG"},
                {"CK","CK/COK"},
                {"CR","CR/CRI"},
                {"HR","HR/HRV"},
                {"CU","CU/CUB"},
                {"CW","CW/CUW"},
                {"CY","CY/CYP"},
                {"CZ","CZ/CZE"},
                {"CI","CI/CIV"},
                {"DK","DK/DNK"},
                {"DJ","DJ/DJI"},
                {"DM","DM/DMA"},
                {"DO","DO/DOM"},
                {"EC","EC/ECU"},
                {"EG","EG/EGY"},
                {"SV","SV/SLV"},
                {"GQ","GQ/GNQ"},
                {"ER","ER/ERI"},
                {"EE","EE/EST"},
                {"SZ","SZ/SWZ"},
                {"ET","ET/ETH"},
                {"FK","FK/FLK"},
                {"FO","FO/FRO"},
                {"FJ","FJ/FJI"},
                {"FI","FI/FIN"},
                {"FR","FR/FRA"},
                {"GF","GF/GUF"},
                {"PF","PF/PYF"},
                {"TF","TF/ATF"},
                {"GA","GA/GAB"},
                {"GM","GM/GMB"},
                {"GE","GE/GEO"},
                {"DE","DE/DEU"},
                {"GH","GH/GHA"},
                {"GI","GI/GIB"},
                {"GR","GR/GRC"},
                {"GL","GL/GRL"},
                {"GD","GD/GRD"},
                {"GP","GP/GLP"},
                {"GU","GU/GUM"},
                {"GT","GT/GTM"},
                {"GG","GG/GGY"},
                {"GN","GN/GIN"},
                {"GW","GW/GNB"},
                {"GY","GY/GUY"},
                {"HT","HT/HTI"},
                {"HM","HM/HMD"},
                {"VA","VA/VAT"},
                {"HN","HN/HND"},
                {"HK","HK/HKG"},
                {"HU","HU/HUN"},
                {"IS","IS/ISL"},
                {"IN","IN/IND"},
                {"ID","ID/IDN"},
                {"IR","IR/IRN"},
                {"IQ","IQ/IRQ"},
                {"IE","IE/IRL"},
                {"IM","IM/IMN"},
                {"IL","IL/ISR"},
                {"IT","IT/ITA"},
                {"JM","JM/JAM"},
                {"JP","JP/JPN"},
                {"JE","JE/JEY"},
                {"JO","JO/JOR"},
                {"KZ","KZ/KAZ"},
                {"KE","KE/KEN"},
                {"KI","KI/KIR"},
                {"KP","KP/PRK"},
                {"KR","KR/KOR"},
                {"KW","KW/KWT"},
                {"KG","KG/KGZ"},
                {"LA","LA/LAO"},
                {"LV","LV/LVA"},
                {"LB","LB/LBN"},
                {"LS","LS/LSO"},
                {"LR","LR/LBR"},
                {"LY","LY/LBY"},
                {"LI","LI/LIE"},
                {"LT","LT/LTU"},
                {"LU","LU/LUX"},
                {"MO","MO/MAC"},
                {"MG","MG/MDG"},
                {"MW","MW/MWI"},
                {"MY","MY/MYS"},
                {"MV","MV/MDV"},
                {"ML","ML/MLI"},
                {"MT","MT/MLT"},
                {"MH","MH/MHL"},
                {"MQ","MQ/MTQ"},
                {"MR","MR/MRT"},
                {"MU","MU/MUS"},
                {"YT","YT/MYT"},
                {"MX","MX/MEX"},
                {"FM","FM/FSM"},
                {"MD","MD/MDA"},
                {"MC","MC/MCO"},
                {"MN","MN/MNG"},
                {"ME","ME/MNE"},
                {"MS","MS/MSR"},
                {"MA","MA/MAR"},
                {"MZ","MZ/MOZ"},
                {"MM","MM/MMR"},
                {"NA","NA/NAM"},
                {"NR","NR/NRU"},
                {"NP","NP/NPL"},
                {"NL","NL/NLD"},
                {"NC","NC/NCL"},
                {"NZ","NZ/NZL"},
                {"NI","NI/NIC"},
                {"NE","NE/NER"},
                {"NG","NG/NGA"},
                {"NU","NU/NIU"},
                {"NF","NF/NFK"},
                {"MP","MP/MNP"},
                {"NO","NO/NOR"},
                {"OM","OM/OMN"},
                {"PK","PK/PAK"},
                {"PW","PW/PLW"},
                {"PS","PS/PSE"},
                {"PA","PA/PAN"},
                {"PG","PG/PNG"},
                {"PY","PY/PRY"},
                {"PE","PE/PER"},
                {"PH","PH/PHL"},
                {"PN","PN/PCN"},
                {"PL","PL/POL"},
                {"PT","PT/PRT"},
                {"PR","PR/PRI"},
                {"QA","QA/QAT"},
                {"MK","MK/MKD"},
                {"RO","RO/ROU"},
                {"RU","RU/RUS"},
                {"RW","RW/RWA"},
                {"RE","RE/REU"},
                {"BL","BL/BLM"},
                {"SH","SH/SHN"},
                {"KN","KN/KNA"},
                {"LC","LC/LCA"},
                {"MF","MF/MAF"},
                {"PM","PM/SPM"},
                {"VC","VC/VCT"},
                {"WS","WS/WSM"},
                {"SM","SM/SMR"},
                {"ST","ST/STP"},
                {"SA","SA/SAU"},
                {"SN","SN/SEN"},
                {"RS","RS/SRB"},
                {"SC","SC/SYC"},
                {"SL","SL/SLE"},
                {"SG","SG/SGP"},
                {"SX","SX/SXM"},
                {"SK","SK/SVK"},
                {"SI","SI/SVN"},
                {"SB","SB/SLB"},
                {"SO","SO/SOM"},
                {"ZA","ZA/ZAF"},
                {"GS","GS/SGS"},
                {"SS","SS/SSD"},
                {"ES","ES/ESP"},
                {"LK","LK/LKA"},
                {"SD","SD/SDN"},
                {"SR","SR/SUR"},
                {"SJ","SJ/SJM"},
                {"SE","SE/SWE"},
                {"CH","CH/CHE"},
                {"SY","SY/SYR"},
                {"TW","TW/TWN"},
                {"TJ","TJ/TJK"},
                {"TZ","TZ/TZA"},
                {"TH","TH/THA"},
                {"TL","TL/TLS"},
                {"TG","TG/TGO"},
                {"TK","TK/TKL"},
                {"TO","TO/TON"},
                {"TT","TT/TTO"},
                {"TN","TN/TUN"},
                {"TR","TR/TUR"},
                {"TM","TM/TKM"},
                {"TC","TC/TCA"},
                {"TV","TV/TUV"},
                {"UG","UG/UGA"},
                {"UA","UA/UKR"},
                {"AE","AE/ARE"},
                {"GB","GB/GBR"},
                {"UM","UM/UMI"},
                {"US","US/USA"},
                {"UY","UY/URY"},
                {"UZ","UZ/UZB"},
                {"VU","VU/VUT"},
                {"VE","VE/VEN"},
                {"VN","VN/VNM"},
                {"VG","VG/VGB"},
                {"VI","VI/VIR"},
                {"WF","WF/WLF"},
                {"EH","EH/ESH"},
                {"YE","YE/YEM"},
                {"ZM","ZM/ZMB"},
                {"ZW","ZW/ZWE"},
                {"AX","AX/ALA"}

                };
            if (!string.IsNullOrEmpty(Country))
            {
               var data= map.FirstOrDefault(x => x.Key == Country);
                return data.Value;
            }
            return "";
        }

    }
}