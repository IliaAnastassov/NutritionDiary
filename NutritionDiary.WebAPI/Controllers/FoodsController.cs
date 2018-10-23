using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.Entities;
using NutritionDiary.WebAPI.Models;

namespace NutritionDiary.WebAPI.Controllers
{
    public class FoodsController : ApiController
    {
        private INutritionDiaryRepository _repository;
        private ModelFactory _modelFactory;

        public FoodsController(INutritionDiaryRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<FoodModel> Get(bool includeMeasures = true)
        {
            _modelFactory = new ModelFactory(Request);

            IQueryable<Food> query;

            if (includeMeasures)
            {
                query = _repository.GetAllFoodsWithMeasures();
            }
            else
            {
                query = _repository.GetAllFoods();
            }

            var foods = query.OrderBy(f => f.Description)
                             .Take(25)
                             .ToList()
                             .Select(f => _modelFactory.Create(f));

            return foods;
        }

        public FoodModel Get(int foodId)
        {
            _modelFactory = new ModelFactory(Request);

            var food = _repository.GetFood(foodId);
            return _modelFactory.Create(food);
        }
    }
}
