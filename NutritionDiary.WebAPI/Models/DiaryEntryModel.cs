using System.Collections.Generic;

namespace NutritionDiary.WebAPI.Models
{
    public class DiaryEntryModel
    {
        public IEnumerable<LinkModel> Links { get; set; }
        public string FoodDescription { get; set; }
        public string MeasureDescription { get; set; }
        public string MeasureUrl { get; set; }
        public double Quantity { get; set; }
    }
}