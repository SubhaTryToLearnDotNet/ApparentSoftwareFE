using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apparent.Model
{
    public class EmailVerifyModel
    {
        public string Email { get; set; }
        public long Timestamp { get; set; }
        public string hrflink { get; set; }
    }
}