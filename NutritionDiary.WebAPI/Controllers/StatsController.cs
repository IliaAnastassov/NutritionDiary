using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NutritionDiary.Data.Interfaces;

namespace NutritionDiary.WebAPI.Controllers
{
    [RoutePrefix("api/stats")]
    public class StatsController : BaseApiController
    {
        public StatsController(INutritionDiaryRepository repository)
            : base(repository)
        {
        }

        [Route("")]
        public IHttpActionResult Get()
        {
            var model = new
            {
                FoodCount = Repository.GetAllFoods().Count(),
                UserCount = Repository.GetApiUsers().Count()
            };

            return Ok(model);
        }

        [Route("~/api/stat/{id:int}")]
        public IHttpActionResult Get(int id)
        {
            IHttpActionResult result;

            if (id == 1)
            {
                var model = new
                {
                    FoodCount = Repository.GetAllFoods().Count()
                };

                result = Ok(model);
            }
            else if (id == 2)
            {
                var model = new
                {
                    UserCount = Repository.GetApiUsers().Count()
                };

                result = Ok(model);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }

        [Route("~/api/stat/{name:alpha}")]
        public IHttpActionResult Get(string name)
        {
            IHttpActionResult result;

            if (name == "foods")
            {
                var model = new
                {
                    FoodCount = Repository.GetAllFoods().Count()
                };

                result = Ok(model);
            }
            else if (name == "users")
            {
                var model = new
                {
                    UserCount = Repository.GetApiUsers().Count()
                };

                result = Ok(model);
            }
            else
            {
                result = NotFound();
            }

            return result;
        }
    }
}
