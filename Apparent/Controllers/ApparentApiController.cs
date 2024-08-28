using Apparent.CustomOAuth;
using Apparent.Model;
using Apparent.Services;
using Azure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;

namespace Apparent.Controllers
{

    [RoutePrefix("api/companydetails")]
    public class ApparentApiController : ApiController
    {
        private readonly IApiService _apiService;
        public ApparentApiController() 
        {
            _apiService = new ApiService();
        }

        [HttpPost]
        [Route("Sales")]
        public async Task <IHttpActionResult> ProductSalesDetails(CompayAccessRequestModel model)
        {
            //var token = Request.Headers.Authorization?.Parameter;
            //if (string.IsNullOrEmpty(token))
            //{
            //    var response = Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Token is missing or empty");
            //    return ResponseMessage(response);
            //}
            //var tokenHandler = new JwtSecurityTokenHandler();
            //DateTime expiration;
            //try
            //{
            //    var jwtToken = tokenHandler.ReadJwtToken(token);
            //    expiration = jwtToken.ValidTo;
            //}
            //catch (Exception ex)
            //{
            //    return Unauthorized();
            //}
            //if (expiration < DateTime.UtcNow)
            //{
            //    var response = Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Session has expired.");
            //    return ResponseMessage(response);
            //}

            if (model == null)
            {
                return Content(HttpStatusCode.OK, "Model should not be null here..");
            }
            else
            {
                ProductSalseResponse respons = await _apiService.GetSalesDetails(model);
                if (respons != null && respons.status =="success")
                {
                    return Content(HttpStatusCode.OK, respons, new JsonMediaTypeFormatter(), "application/json");
                }
                else if(respons != null && respons.status_code == 404)
                {
                    return Content(HttpStatusCode.OK, respons, new JsonMediaTypeFormatter(), "application/json");
                }
                else
                {
                    return BadRequest();
                }
            }
           
        }

    }
}
