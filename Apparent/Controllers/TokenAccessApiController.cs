using Apparent.CustomOAuth;
using Apparent.DBContext;
using Apparent.Model;
using Apparent.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace Apparent.Controllers
{
    [RoutePrefix("api/token")]
    public class TokenAccessApiController : ApiController
    {
        private readonly IApiService _apiService;
        public TokenAccessApiController() 
        {
            _apiService = new ApiService();
        }
        [HttpPost]
        [Route("create")]
        public IHttpActionResult create(CompayAccessRequestModel model)
        {
            if (model == null)
            {
                return BadRequest("Model cannot be null");
            }

            // Authenticate the user
            var isAuthenticated = _apiService.AuthenticateUser(model);
            if (isAuthenticated == null)
            {
                return Unauthorized();
            }

            // Generate JWT token
            var token = JwtManager.GenerateToken(model);

            // Return the token
            return Ok(new { token });
        }
    
    }
}
