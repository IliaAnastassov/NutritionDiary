using System.Linq;
using System.Web.Http;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.WebAPI.Models;

namespace NutritionDiary.WebAPI.Controllers
{
    public class MeasuresV2Controller : BaseApiController
    {
        public MeasuresV2Controller(INutritionDiaryRepository repository)
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

            var models = measures.Select(m => ModelFactory.CreateVersion2(m));
            return Ok(models);
        }

        public IHttpActionResult Get(int foodId, int measureId)
        {
            var measure = Repository.GetMeasure(measureId);

            if (measure == null)
            {
                return NotFound();
            }

            MeasureV2Model model;
            if (foodId == measure.Food.Id)
            {
                model = ModelFactory.CreateVersion2(measure);
            }
            else
            {
                return NotFound();
            }

            return Ok(model);
        }
    }
}
