﻿using System;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
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

            CreateCustomMediaTypes(config.Formatters.JsonFormatter);

            // Enable DI for filters / attributes
            config.Services.Add(typeof(IFilterProvider), new UnityFilterProvider(UnityConfig.Container));

            // Replace default controller selector
            config.Services.Replace(typeof(IHttpControllerSelector), new NutritionDiaryControllerSelector(config));

            // CORS support
            var policyProvider = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(policyProvider);

#if !DEBUG
            // Force HTTPS on entire Web API
            config.Filters.Add(new RequireHttpsAttribute());
#endif

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