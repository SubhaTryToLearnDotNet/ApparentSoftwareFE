using Apparent.Model;
using Apparent.Repository;
using Apparent.Services;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;


namespace Apparent.Controllers
{
    [RoutePrefix("api/ApparentPay")]
    public class ApiPaymentServiceController : ApiController
    {
        private readonly IApiPaymentService _apiPaymentService;
        public ApiPaymentServiceController()
        {
            _apiPaymentService = new ApiPaymentService();
        }

        [HttpPost]
        [Route("payment")]
        public  IHttpActionResult payment(PaymentRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return Content(HttpStatusCode.BadRequest, "Invalid data");
            }
           string RequestKey =  _apiPaymentService.CreatePaymentRequest(requestModel);
            
            var result = new PaymentResponseModel();
            if (RequestKey != null) {
                string baseUrl = GetBaseUrl();
                result.status = "success";
                result.statusCode = 200;
                result.redirectUrl = $"{baseUrl}/Service/Pay?Key={RequestKey}";
                return Content(HttpStatusCode.OK, result, new JsonMediaTypeFormatter(), "application/json");
            }
            else
            {
                return Content(HttpStatusCode.BadRequest, result, new JsonMediaTypeFormatter(), "application/json");
            }
        }
        private string GetBaseUrl()
        {
            var request = Request;
            var uri = request.RequestUri;
            var baseUrl = $"{uri.Scheme}://{uri.Host}:{uri.Port}";
            return baseUrl;
        }

    }
}
