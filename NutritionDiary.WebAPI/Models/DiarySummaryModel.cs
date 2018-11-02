using System;

namespace NutritionDiary.WebAPI.Models
{
    public class DiarySummaryModel
    {
        public DateTime DiaryDate { get; set; }
        public int TotalCalories { get; set; }
    }
}