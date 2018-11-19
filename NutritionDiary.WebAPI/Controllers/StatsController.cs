using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NutritionDiary.Data.Interfaces;

namespace NutritionDiary.WebAPI.Controllers
{
    public class StatsController : BaseApiController
    {
        public StatsController(INutritionDiaryRepository repository)
            : base(repository)
        {
        }

        [Route("api/stats")]
        public IHttpActionResult Get()
        {
            var model = new
            {
                FoodCount = Repository.GetAllFoods().Count()
            };

            return Ok(model);
        }
    }
}
