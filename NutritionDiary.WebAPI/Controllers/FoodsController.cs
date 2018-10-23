using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public IEnumerable<FoodModel> Get(bool includeMeasures = true)
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
                             .ToList()
                             .Select(f => ModelFactory.Create(f));

            return foods;
        }

        public FoodModel Get(int foodId)
        {
            var food = Repository.GetFood(foodId);
            return ModelFactory.Create(food);
        }
    }
}
