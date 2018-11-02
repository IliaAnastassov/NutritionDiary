using System;

namespace NutritionDiary.WebAPI.Models
{
    public class DiarySummaryModel
    {
        public DateTime DiaryDate { get; set; }
        public double TotalCalories { get; set; }
    }
}