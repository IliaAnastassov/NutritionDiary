using System;
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
        private const string CONTROLLER_KEY = "controller";
        private const string VERSIONED_CONTROLLER_FORMAT = "{0}V{1}";
        private const string VERSION_QUERY_KEY = "v";
        private const string VERSION_ACCEPT_HEADER_KEY = "version";
        private const string DEFAULT_VERSION = "1";
        private const string VERSION_HEADER_NAME = "X-NutritionDiary-Version";
        private const string JSON_MEDIA_TYPE = "application/json";
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
            var controllerName = (string)routeData.Values[CONTROLLER_KEY];

            if (controllers.TryGetValue(controllerName, out HttpControllerDescriptor descriptor))
            {
                var version = GetVersionFromAcceptHeader(request);
                var versionedControllerName = string.Format(VERSIONED_CONTROLLER_FORMAT, controllerName, version);

                if (controllers.TryGetValue(versionedControllerName, out HttpControllerDescriptor versionedControllerDescriptor))
                {
                    descriptor = versionedControllerDescriptor;
                }
            }

            return descriptor;
        }

        private string GetVersionFromAcceptHeader(HttpRequestMessage request)
        {
            var version = DEFAULT_VERSION;
            var acceptHeader = request.Headers.Accept;

            foreach (var mimeType in acceptHeader)
            {
                if (mimeType.MediaType == JSON_MEDIA_TYPE)
                {
                    var versionParameter = mimeType.Parameters
                                                   .FirstOrDefault(p => p.Name.Equals(VERSION_ACCEPT_HEADER_KEY, StringComparison.OrdinalIgnoreCase));

                    if (versionParameter != null && !string.IsNullOrWhiteSpace(versionParameter.Value))
                    {
                        version = versionParameter.Value;
                    }
                }
            }

            return version;
        }

        private string GetVersionFromRequestHeader(HttpRequestMessage request)
        {
            var version = DEFAULT_VERSION;

            if (request.Headers.Contains(VERSION_HEADER_NAME))
            {
                var versionHeaderValue = request.Headers.GetValues(VERSION_HEADER_NAME).FirstOrDefault();

                if (!string.IsNullOrWhiteSpace(versionHeaderValue))
                {
                    version = versionHeaderValue;
                }
            }

            return version;
        }

        private string GetVersionFromQueryString(HttpRequestMessage request)
        {
            var query = HttpUtility.ParseQueryString(request.RequestUri.Query);
            var version = query[VERSION_QUERY_KEY];

            if (string.IsNullOrWhiteSpace(version))
            {
                version = DEFAULT_VERSION;
            }

            return version;
        }
    }
}