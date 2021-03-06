﻿using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace NutritionDiary.WebAPI.Services
{
    public class NutritionDiaryControllerSelector : DefaultHttpControllerSelector
    {
        private const string VERSIONED_CONTROLLER_FORMAT = "{0}V{1}";
        private const string VERSION_QUERY_KEY = "v";
        private const string VERSION_ACCEPT_HEADER_KEY = "version";
        private const string DEFAULT_VERSION = "1";
        private const string VERSION_HEADER_NAME = "X-NutritionDiary-Version";
        private const string JSON_MEDIA_TYPE = "application/json";
        private const string VERSION_REGEX = @"application\/vnd\.nutritiondiary\.([a-z]+)\.v([0-9]+)\+json";
        private const int VERSION_REGEX_GROUP_INDEX = 2;

        public NutritionDiaryControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
        }

        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            var controllers = GetControllerMapping();
            var controllerDescriptor = base.SelectController(request);

            var version = GetVersionFromMediaType(request);
            var versionedControllerName = string.Format(VERSIONED_CONTROLLER_FORMAT, controllerDescriptor.ControllerName, version);

            if (controllers.TryGetValue(versionedControllerName, out HttpControllerDescriptor versionedControllerDescriptor))
            {
                controllerDescriptor = versionedControllerDescriptor;
            }

            return controllerDescriptor;
        }

        private string GetVersionFromMediaType(HttpRequestMessage request)
        {
            var version = DEFAULT_VERSION;
            var acceptHeader = request.Headers.Accept;
            var expression = new Regex(VERSION_REGEX, RegexOptions.IgnoreCase);

            foreach (var mime in acceptHeader)
            {
                var match = expression.Match(mime.MediaType);

                if (match.Success)
                {
                    version = match.Groups[VERSION_REGEX_GROUP_INDEX].Value;
                }
            }

            return version;
        }

        private string GetVersionFromAcceptHeader(HttpRequestMessage request)
        {
            var version = DEFAULT_VERSION;
            var acceptHeader = request.Headers.Accept;

            foreach (var mime in acceptHeader)
            {
                if (mime.MediaType == JSON_MEDIA_TYPE)
                {
                    var versionParameter = mime.Parameters
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