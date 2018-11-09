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

namespace NutritionDiary.WebAPI.Filters
{
    /// <summary>
    /// Gives support for Basic Authorization
    /// </summary>
    public class NutritionDiaryAuthorizeAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return;
            }

            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                if (authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase)
                    && !string.IsNullOrWhiteSpace(authHeader.Parameter))
                {
                    var rawCredentials = authHeader.Parameter;
                    var encoding = Encoding.GetEncoding("iso-8859-1");
                    var credentials = encoding.GetString(Convert.FromBase64String(rawCredentials));
                    var username = credentials.Split(':').First();
                    var password = credentials.Split(':').Last();

                    if (Login(username, password))
                    {
                        var principal = new GenericPrincipal(new GenericIdentity(username), roles: null);
                        Thread.CurrentPrincipal = principal;
                        return;
                    }
                }
            }

            HandleUnauthorized(actionContext);
        }

        private bool Login(string username, string password)
        {
            // TODO: add membership provider
            return true;
        }

        private void HandleUnauthorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Nutrition Diary' location='~/account/login'");
        }
    }
}