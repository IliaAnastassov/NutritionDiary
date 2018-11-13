using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace NutritionDiary.WebAPI.Services
{
    public class NutritionDiaryControllerSelector : DefaultHttpControllerSelector
    {
        private HttpConfiguration _configuration;

        public NutritionDiaryControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var controllers = GetControllerMapping();
            var routeData = request.GetRouteData();
            var controllerName = (string)routeData.Values["controller"];

            if (controllers.TryGetValue(controllerName, out HttpControllerDescriptor descriptor))
            {
                var version = GetVersionFrom(request);
                var versionedControllerName = $"{controllerName}V{version}";

                if (controllers.TryGetValue(versionedControllerName, out HttpControllerDescriptor versionedControllerDescriptor))
                {
                    descriptor = versionedControllerDescriptor;
                }
            }

            return descriptor;
        }

        private string GetVersionFrom(HttpRequestMessage request)
        {
            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
            var version = query["v"];

            if (string.IsNullOrWhiteSpace(version))
            {
                version = "1";
            }

            return version;
        }
    }
}