using Apparent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Apparent.Services
{
    internal interface IApiService
    {
        Task<ProductSalseResponse> GetSalesDetails(CompayAccessRequestModel model);

        CompayAccessRequestModel AuthenticateUser(CompayAccessRequestModel model);
    }
}
