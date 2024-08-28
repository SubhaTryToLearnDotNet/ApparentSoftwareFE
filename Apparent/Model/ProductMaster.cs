using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Apparent.Model
{
    public class ProductMasterDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company_Email { get; set; }
        public List<string> Getcategory { get; set; }
        public string AboutProduct { get; set; }
        public string SupportLanguages { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }
    }
    public class ProductMaster : CompanyMaster
    {
         public ProductMaster() 
         {
            Product_imagesFile = new List<ProductImage>();
            Product_videosFile = new List<Video>();
            subscriptions = new List<subscription>();
            Getcategory = new List<string>();
            GetProduct_Comments = new List<Product_Comment>();
            GetPrice_Comments = new List<Price_Comment>();
            GetFeatures_Comments =   new List<Features_Comment>();

            subscription_Tokens = new List<subscription_token>();
         }
        public int id { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company_Email { get; set; }
        public string ProductId { get; set; }
        public string SellerId { get;set; }
        public List<string> Getcategory { get; set; }
        public string AboutProduct { get; set; }
        public string SupportLanguages { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Category { get; set; }
        public string ProductName { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }
        public string token { get; set; }
        public DataTable ProductVideo { get; set; }
        public DataTable SellerDeatils { get; set; }

        public string VideoFile { get; set; }
        public HttpPostedFileBase ProductImage { get; set; }
        public string ProductIcon { get; set; }
        public HttpPostedFileBase[]  ScreenshotFile { get; set; }
        public string Images { get; set; }

        //public HttpPostedFileBase[] PDF { get; set; }
        //public string PdfId { get; set; }
       

        public string ThumbnailId { get; set; }
        public DataTable DtProductList { get; set; }
        public DataTable PdfFileList { get; set; }
        public DataTable DtProductDetails { get; set; }
        public DataTable VideoList { get;set;}
        public DataTable ImagesList { get; set; }
        public DataTable Token_List { get; set; }
        public DataTable Features { get; set; }

        public DataTable Packeglist { get; set; }
        public string IndividualPrice { get; set; }
        public string StandardPrice { get; set; }
        public string BusinessPrice { get; set; }
        public string BusinessPremiumPrice { get; set; }
        public string EnterprisePrice { get; set; }
        public string Notes { get; set; }
        public string Admin_Approval { get; set; }
        public string Request_status { get; set; }

        public DateTime date { get; set; }


        public string respons_result { get; set; }

        public string ImagesComment { get; set; }
        public string PricingComment { get; set;}
        public string FeaturesComment { get; set; }
        public List<ProductImage> Product_imagesFile { get; set; }
        public List<Video> Product_videosFile { get; set; }
        public List<string> Lang { get; set; }
        public List<string> Features1 { get; set; }
        public List<string> Features2 { get; set; }
        public List<string> Features3 { get; set; }
        public List<string> featuresg1_Added { get; set; }
        public List<string> featuresg2_Added { get; set; }
        public List<string> featuresg3_Added { get; set; }
        public string Mergin { get; set; }
        public string subcrp { get; set; }
        public string token_master { get; set; }
        public string subscription_comment { get; set; }
        public List<subscription> subscriptions { get; set; }
        public List<subscription_token> subscription_Tokens { get; set; }
        public List<Product_Comment> GetProduct_Comments { get; set; }
        public List<Price_Comment> GetPrice_Comments { get; set; }
        public List<Features_Comment>GetFeatures_Comments  { get; set; }
        public string CategoryValue { get; set; }
        public string LanguageValue { get; set; }
        public string[] AllBlobImgAddress { get; set; }
        public string[] AllBlobVideoAddress { get; set; }
        public string Product_Url { get; set; }
        public subscription GetSubscription {  get; set; }  = new subscription();


	}



    public class features : ProductMaster
    {
        public string FeaturesGroup1 { get; set; }
        public string FeaturesGroup2 { get; set; }
        public string FeaturesGroup3 { get; set; }
        public string Add_featuresg1 { get; set; }
        public string Add_featuresg2 { get; set; }
        public string Add_featuresg3 { get; set; }

        public string Features_Commentgroup { get; set; }
    }
        public class ProductImage
    {
        public int ImageId { get; set; }
        public string ImagesPath { get; set; }
        public string Product_Id { get; set; }
        

    }
    public class Video
    {
        public int VdoFileId { get; set; }
        public string Path { get; set; }
        public string Product_Id { get; set; }
    }
    public class subscription
    {
        public string Plan_Name { get; set; }
        public decimal Price { get; set; }
        public string Plan_tenure { get; set; }
        public string Description { get; set; }
        public string Plan_uniqueId { get; set; }
        public string  token { get; set; }
        public int Token_Count { get; set; }
        public int Sold_Count { get; set; }
        public string[] AdditionalFeatures { get; set; }
    }
    public class Product_Comment
    {
        public string Product_ID { get; set; }
        public string Admin_ID { get; set; }
        public string Seller_ID { get; set; }
        public string Comment { get; set; }
        public string Type { get; set; }
        public DateTime Comment_Time { get; set; }
    }
    public class Price_Comment: Product_Comment
    {
        
    }
    public class Features_Comment : Product_Comment
    {

    }

    public class AccessMeRequestModel
    {
        public UserInfo UserInfo { get; set; }
        public Subscription Subscription { get; set; }
    }
    public class UserInfo
    {
        [Required]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
    public class Subscription
    {
        [Required]
        public int ProductId { get; set; } 

		[Required]
		public int PlanId { get; set; } =0;

		[Required]
		public string PlanName { get; set; } = string.Empty;

    }

}
