namespace NutritionDiary.WebAPI.Models
{
    public class DiaryEntryModel
    {
        public string Url { get; set; }
        public FoodModel FoodItem { get; set; }
        public MeasureModel Measure { get; set; }
        public double Quantity { get; set; }
    }
}