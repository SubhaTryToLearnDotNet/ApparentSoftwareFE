using Apparent.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apparent.Services
{
    public interface IApiPaymentService
    {
        string CreatePaymentRequest(PaymentRequestModel model);
        Respons CheckRequestKey(string Key);

        RedirectRespons AddApiPaymentRespons(RedirectRespons model);

         RedirectRespons AddAppPayRespons(RedirectRespons model);


    }
}
