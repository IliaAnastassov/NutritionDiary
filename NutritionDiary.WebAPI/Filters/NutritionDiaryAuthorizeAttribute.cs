using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using NutritionDiary.Data.Interfaces;
using Unity.Attributes;

namespace NutritionDiary.WebAPI.Filters
{
    /// <summary>
    /// Gives support for Basic Authorization
    /// </summary>
    public class NutritionDiaryAuthorizeAttribute : AuthorizationFilterAttribute
    {
        private const string API_KEY = "apikey";
        private const string TOKEN = "token";
        private bool _authorizePerUser;

        public NutritionDiaryAuthorizeAttribute(bool authorizePerUser = true)
        {
            _authorizePerUser = authorizePerUser;
        }

        [Dependency]
        public INutritionDiaryRepository Repository { get; set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var queryString = HttpUtility.ParseQueryString(actionContext.Request.RequestUri.Query);
            var apiKey = queryString[API_KEY];
            var token = queryString[TOKEN];

            if (!string.IsNullOrWhiteSpace(apiKey) && !string.IsNullOrWhiteSpace(token))
            {
                var authToken = Repository.GetAuthToken(token);

                if (authToken != null && authToken.ApiUser.AppId == apiKey && authToken.Expiration > DateTime.UtcNow)
                {
                    if (_authorizePerUser)
                    {
                        AuthorizeUser(actionContext);
                    }
                }
            }
            else
            {
                HandleUnauthorized(actionContext);
            }
        }

        private void AuthorizeUser(HttpActionContext actionContext)
        {
            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var authHeader = actionContext.Request.Headers.Authorization;

                if (ValidateAuthHeader(authHeader))
                {
                    var rawCredentials = authHeader.Parameter;
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    var credentials = encoding.GetString(Convert.FromBase64String(rawCredentials));
                    var username = credentials.Split(':').First();
                    var password = credentials.Split(':').Last();

                    if (Login(username, password))
                    {
                        var identity = new GenericIdentity(username);
                        Thread.CurrentPrincipal = new GenericPrincipal(identity, roles: null);
                    }
                    else
                    {
                        HandleUnauthorized(actionContext);
                    }
                }
                else
                {
                    HandleUnauthorized(actionContext);
                }
            }
        }

        private bool ValidateAuthHeader(System.Net.Http.Headers.AuthenticationHeaderValue authHeader)
        {
            return authHeader != null
                && authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase)
                && !string.IsNullOrWhiteSpace(authHeader.Parameter);
        }

        private bool Login(string username, string password)
        {
            // TODO: add membership provider
            return true;
        }

        private void HandleUnauthorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);

            if (_authorizePerUser)
            {
                actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Nutrition Diary' location='~/account/login'");
            }
        }
    }
}