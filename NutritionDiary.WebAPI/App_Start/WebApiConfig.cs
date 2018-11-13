using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using System.Web.Http.Filters;
using Newtonsoft.Json.Serialization;
using NutritionDiary.WebAPI.Services;

namespace NutritionDiary.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Foods",
                routeTemplate: "api/nutrition/foods/{foodid}",
                defaults: new { controller = "foods", foodid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Measures",
                routeTemplate: "api/nutrition/foods/{foodid}/measures/{measureid}",
                defaults: new { controller = "measures", measureid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "Diaries",
                routeTemplate: "api/user/diaries/{diaryid}",
                defaults: new { controller = "diaries", diaryid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DiaryEntries",
                routeTemplate: "api/user/diaries/{diaryid}/entries/{entryid}",
                defaults: new { controller = "diaryentries", entryid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DiarySummary",
                routeTemplate: "api/user/diaries/{diaryid}/summary",
                defaults: new { controller = "diarysummary" }
            );

            config.Routes.MapHttpRoute(
                name: "Token",
                routeTemplate: "api/token",
                defaults: new { controller = "token" }
            );

            // CORS support
            var policyProvider = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(policyProvider);

            // Enable DI for filters / attributes
            config.Services.Add(typeof(IFilterProvider), new UnityFilterProvider(UnityConfig.Container));

            // Replace default controller selector
            config.Services.Replace(typeof(IHttpControllerSelector), new NutritionDiaryControllerSelector(config));

#if !DEBUG
            // Force HTTPS on entire Web API
            config.Filters.Add(new RequireHttpsAttribute());
#endif
        }
    }
}
