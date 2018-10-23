using System.Linq;
using System.Web.Http;
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

        public IHttpActionResult Get(int foodId)
        {
            var measures = Repository.GetMeasuresForFood(foodId)
                                     .OrderBy(m => m.Description)
                                     .ToList();

            if (measures == null)
            {
                return NotFound();
            }

            var models = measures.Select(m => ModelFactory.Create(m));
            return Ok(models);
        }

        public IHttpActionResult Get(int foodId, int measureId)
        {
            var measure = Repository.GetMeasure(measureId);

            if (measure == null)
            {
                return NotFound();
            }

            MeasureModel model;
            if (foodId == measure.Food.Id)
            {
                model = ModelFactory.Create(measure);
            }
            else
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}
