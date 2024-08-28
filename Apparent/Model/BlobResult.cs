using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Apparent.Model
{
    public class BlobResult
    {
        public string Uri { get; set; }
        public string BlobName { get; set; }
        public string BlobContainerName { get; set; }
        public string BlobAccountName { get; set; }
    }
}