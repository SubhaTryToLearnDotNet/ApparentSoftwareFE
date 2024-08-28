using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Apparent.DBContext;

namespace Apparent.CustomOAuth
{
    public class CustomOAuthAttribute : AuthorizeAttribute
    {
        private  bool _tokenIsActive;
        private string _status;
        protected override  bool IsAuthorized(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;
            if (!headers.Contains("Authorization"))
            {
                _status = "Unauthorized access.";
                return false; 
            }
            var authHeader = headers.GetValues("Authorization").FirstOrDefault();
            if (authHeader == null || !authHeader.StartsWith("Bearer "))
            {
                _status = "Missing Bearer token";
                return false;
            }
            var token = authHeader.Substring("Bearer ".Length).Trim();

            CompanyDbContext context = new CompanyDbContext();
            _tokenIsActive = context.CheckAccessTokenAsBeareToken(token);
            if (!_tokenIsActive)
            {
                _status = "Invalid Bearer token";
                return false;
            }

            return true;
        }
        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateErrorResponse(
                HttpStatusCode.Unauthorized, _status);
        }
    }
}