using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace NutritionDiary.WebAPI.ActionResults
{
    public class VersionedActionResult<T> : IHttpActionResult
        where T : class
    {
        private HttpRequestMessage _request;
        private T _body;
        private string _version;

        public VersionedActionResult(HttpRequestMessage request, T body, string version)
        {
            _request = request;
            _body = body;
            _version = version;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(_body);
            response.Headers.Add("XXX-OurVersion", _version);
            return Task.FromResult(response);
        }
    }
}