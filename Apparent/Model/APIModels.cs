using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apparent.Model
{
    public class APIResponseModel
    {
        public int StatusCode { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public string ReturnURL { get; set; }

    }
}