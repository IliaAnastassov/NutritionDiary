using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.Entities;
using NutritionDiary.WebAPI.Models;

namespace NutritionDiary.WebAPI.Controllers
{
    public class FoodsController : BaseApiController
    {
        public FoodsController(INutritionDiaryRepository repository)
            : base(repository)
        {
        }

        public IHttpActionResult Get(bool includeMeasures = true)
        {
            IQueryable<Food> query;

            if (includeMeasures)
            {
                query = Repository.GetAllFoodsWithMeasures();
            }
            else
            {
                query = Repository.GetAllFoods();
            }

            var foods = query.OrderBy(f => f.Description)
                             .Take(25)
                             .ToList();

            if (foods == null)
            {
                return NotFound();
            }

            var models = foods.Select(f => ModelFactory.Create(f));
            return Ok(models);
        }

        public IHttpActionResult Get(int foodId)
        {
            var food = Repository.GetFood(foodId);

            if (food == null)
            {
                return NotFound();
            }

            var model = ModelFactory.Create(food);
            return Ok(model);
        }
    }
}
