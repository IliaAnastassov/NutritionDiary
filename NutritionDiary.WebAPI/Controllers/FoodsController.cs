using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using NutritionDiary.Data;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.Entities;

namespace NutritionDiary.WebAPI.Controllers
{
    public class FoodsController : ApiController
    {
        private INutritionDiaryRepository _repository;

        public FoodsController(INutritionDiaryRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Food> Get()
        {
            var foods = _repository.GetAllFoods()
                                   .OrderBy(f => f.Description)
                                   .Take(25)
                                   .ToList();

            return foods;
        }
    }
}
