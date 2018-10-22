using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.WebAPI.Models;

namespace NutritionDiary.WebAPI.Controllers
{
    public class MeasuresController : ApiController
    {
        private INutritionDiaryRepository _repository;
        private ModelFactory _modelFactory;

        public MeasuresController(INutritionDiaryRepository repository)
        {
            _repository = repository;
            _modelFactory = new ModelFactory();
        }

        public IEnumerable<MeasureModel> Get(int foodId)
        {
            var measures = _repository.GetMeasuresForFood(foodId)
                                      .OrderBy(m => m.Description)
                                      .ToList()
                                      .Select(m => _modelFactory.Create(m));

            return measures;
        }

        public MeasureModel Get(int foodId, int measureId)
        {
            var measure = _repository.GetMeasure(measureId);
            MeasureModel model;

            if (foodId == measure.Food?.Id)
            {
                model = _modelFactory.Create(measure);
            }
            else
            {
                model = null;
            }

            return model;
        }
    }
}
