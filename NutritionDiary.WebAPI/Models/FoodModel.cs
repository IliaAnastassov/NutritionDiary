using System.Collections.Generic;

namespace NutritionDiary.WebAPI.Models
{
    public class FoodModel
    {
        public string Description { get; set; }
        public IEnumerable<MeasureModel> Measures { get; set; }
    }
}