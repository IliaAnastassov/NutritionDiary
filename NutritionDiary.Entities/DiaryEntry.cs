using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NutritionDiary.Entities
{
    public class DiaryEntry
    {
        public int Id { get; set; }
        public Food FoodItem { get; set; }
        public Measure Measure { get; set; }
        public double Quantity { get; set; }

        public Diary Diary { get; set; }
    }
}
