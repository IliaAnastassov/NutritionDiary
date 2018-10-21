using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using NutritionDiary.Data;
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
            _modelFactory = new ModelFactory();
        }

        public IEnumerable<FoodModel> Get()
        {
            var foods = _repository.GetAllFoods()
                                   .OrderBy(f => f.Description)
                                   .Take(25)
                                   .ToList()
                                   .Select(f => _modelFactory.Create(f));

            return foods;
        }

        public FoodModel Get(int id)
        {
            var food = _repository.GetFood(id);
            return _modelFactory.Create(food);
        }
    }
}
