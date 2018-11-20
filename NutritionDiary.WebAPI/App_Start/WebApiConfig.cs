using System.Configuration;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Dispatcher;
using System.Web.Http.Filters;
using CacheCow.Server;
using CacheCow.Server.EntityTagStore.SqlServer;
using Newtonsoft.Json.Serialization;
using NutritionDiary.WebAPI.Converters;
using NutritionDiary.WebAPI.Services;
using static NutritionDiary.WebAPI.Utilities.Constants;

namespace NutritionDiary.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: FOODS_ROUTE,
                routeTemplate: "api/nutrition/foods/{foodid}",
                defaults: new { controller = "foods", foodid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: MEASURES_ROUTE,
                routeTemplate: "api/nutrition/foods/{foodid}/measures/{measureid}",
                defaults: new { controller = "measures", measureid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: DIARIES_ROUTE,
                routeTemplate: "api/user/diaries/{diaryid}",
                defaults: new { controller = "diaries", diaryid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: DIARY_ENTRIES_ROUTE,
                routeTemplate: "api/user/diaries/{diaryid}/entries/{entryid}",
                defaults: new { controller = "diaryentries", entryid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: DIARY_SUMMARY_ROUTE,
                routeTemplate: "api/user/diaries/{diaryid}/summary",
                defaults: new { controller = "diarysummary" }
            );

            config.Routes.MapHttpRoute(
                name: TOKEN_ROUTE,
                routeTemplate: "api/token",
                defaults: new { controller = "token" }
            );

            // Web API configuration and services
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new LinkModelConverter());
            CreateCustomMediaTypes(config.Formatters.JsonFormatter);

            // Enable DI for filters / attributes
            config.Services.Add(typeof(IFilterProvider), new UnityFilterProvider(UnityConfig.Container));

            // Replace default controller selector
            config.Services.Replace(typeof(IHttpControllerSelector), new NutritionDiaryControllerSelector(config));

            // CORS support
            var policyProvider = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(policyProvider);

#if !DEBUG
            // Configure Caching/ETag support
            var connectionString = ConfigurationManager.ConnectionStrings[DEFAULT_CONNECTION_KEY].ConnectionString;
            var etagStore = new SqlServerEntityTagStore(connectionString);
            var cacheHandler = new CachingHandler(config, etagStore);
            config.MessageHandlers.Add(cacheHandler); 
#endif

#if !DEBUG
            // Force HTTPS on entire Web API
            config.Filters.Add(new RequireHttpsAttribute());
#endif
        }

        private static void CreateCustomMediaTypes(JsonMediaTypeFormatter jsonFormatter)
        {
            var mediaTypes = new string[]
            {
                "application/vnd.nutritiondiary.food.v1+json",
                "application/vnd.nutritiondiary.measure.v1+json",
                "application/vnd.nutritiondiary.measure.v2+json",
                "application/vnd.nutritiondiary.diary.v1+json",
                "application/vnd.nutritiondiary.diaryEntry.v1+json"
            };

            foreach (var mediaType in mediaTypes)
            {
                jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(mediaType));
            }
        }
    }
}