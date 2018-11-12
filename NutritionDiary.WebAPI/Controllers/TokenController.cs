using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.Entities;
using NutritionDiary.WebAPI.Models;

namespace NutritionDiary.WebAPI.Controllers
{
    public class TokenController : BaseApiController
    {
        public TokenController(INutritionDiaryRepository repository) : base(repository)
        {
        }

        public IHttpActionResult Post([FromBody]TokenRequestModel model)
        {
            try
            {
                var apiUser = Repository.GetApiUser(model.ApiKey);

                if (apiUser != null)
                {
                    // WARNING: this is a way too simplistic approach, DO NOT USE in real life
                    var key = Convert.FromBase64String(apiUser.Secret);
                    var provider = new HMACSHA256(key);
                    var hash = provider.ComputeHash(Encoding.UTF8.GetBytes(apiUser.AppId));
                    var signature = Convert.ToBase64String(hash);

                    if (signature.Equals(model.Signature))
                    {
                        var rawTokenInfo = string.Concat(apiUser.AppId, DateTime.UtcNow.ToString("d"));
                        var rawTokenByte = Encoding.UTF8.GetBytes(rawTokenInfo);
                        var token = provider.ComputeHash(rawTokenByte);

                        var authToken = new AuthToken
                        {
                            Token = Convert.ToBase64String(token),
                            Expiration = DateTime.UtcNow.AddDays(1),
                            ApiUser = apiUser
                        };

                        if (Repository.Insert(authToken) && Repository.Commit())
                        {
                            var authTokenModel = ModelFactory.Create(authToken);
                            return Created(Url.Request.RequestUri, authTokenModel);
                        }
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
