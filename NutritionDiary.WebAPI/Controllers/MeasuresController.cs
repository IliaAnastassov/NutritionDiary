using System.Collections.Generic;
using System.Linq;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.WebAPI.Models;

namespace NutritionDiary.WebAPI.Controllers
{
    public class MeasuresController : BaseApiController
    {
        public MeasuresController(INutritionDiaryRepository repository)
            : base(repository)
        {
        }

        public IEnumerable<MeasureModel> Get(int foodId)
        {
            var measures = Repository.GetMeasuresForFood(foodId)
                                     .OrderBy(m => m.Description)
                                     .ToList()
                                     .Select(m => ModelFactory.Create(m));

            return measures;
        }

        public MeasureModel Get(int foodId, int measureId)
        {
            var measure = Repository.GetMeasure(measureId);
            MeasureModel model;

            if (foodId == measure.Food.Id)
            {
                model = ModelFactory.Create(measure);
            }
            else
            {
                model = null;
            }

            return model;
        }
    }
}
