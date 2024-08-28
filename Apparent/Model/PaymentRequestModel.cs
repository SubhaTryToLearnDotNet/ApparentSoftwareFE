using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Apparent.Model
{
    public class PaymentRequestModel
    {
        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        [Url(ErrorMessage = "Invalid URL format.")]
        public string RedirectUrl { get; set; }
        public string ProductName { get; set; }
        public string IdentityKey { get; set; }
    }

    public class User
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }
    }

    public class PaymentResponseModel
    {
        public int statusCode { get; set; }
        public string status { get; set; }
        public string message { get; set; }
        public string errorDetails { get; set; }     
        public string redirectUrl { get; set; }

    }

    public class Respons: PaymentRequestModel
    {
      
        public string PaymentRequestKey { get; set; }

    }

    public class RedirectRespons
    {
        public string message { get; set; }
        public bool success { get; set; }
        public string user_name { get; set; }
        public string OrderId { get; set; }
        public decimal amount { get; set; }
        public string currency { get; set; }
        public string email { get; set; }
        public string transaction_id { get; set; }
        public string settlement_date { get; set; }
        public string payment_type { get; set; }
        public DateTime? transaction_date { get; set; }
    }
}