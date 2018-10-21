namespace NutritionDiary.WebAPI.Models
{
    public class MeasureModel
    {
        public string Description { get; set; }
        public int Calories { get; set; }
        public double TotalFat { get; set; }
        public double Protein { get; set; }
        public double Carbohydrates { get; set; }
        public double Fiber { get; set; }
        public double Sugar { get; set; }
    }
}