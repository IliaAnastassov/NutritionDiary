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
        private const string DEFAULT_VERSION = "1";
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
                var version = GetVersionFrom(request);
                var versionedControllerName = string.Format(VERSIONED_CONTROLLER_FORMAT, controllerName, version);

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
            var version = query[VERSION_QUERY_KEY];

            if (string.IsNullOrWhiteSpace(version))
            {
                version = DEFAULT_VERSION;
            }

            return version;
        }
    }
}