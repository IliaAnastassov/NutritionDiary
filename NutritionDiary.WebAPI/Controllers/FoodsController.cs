using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NutritionDiary.Data;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.Entities;

namespace NutritionDiary.WebAPI.Controllers
{
    public class FoodsController : ApiController
    {
        private INutritionDiaryRepository _repository;

        public FoodsController()
        {
            _repository = new NutritionDiaryRepository();
        }

        public FoodsController(INutritionDiaryRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Food> Get()
        {
            return _repository.GetAllFoods()
                              .OrderBy(f => f.Description)
                              .Take(25)
                              .ToList();
        }
    }
}
