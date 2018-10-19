using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.Entities;

namespace NutritionDiary.Data
{
    public class NutritionDiaryRepository : INutritionDiaryRepository
    {
        public IEnumerable<Food> GetAllFoods()
        {
            using (var db = new NutritionDiaryDb())
            {
                return db.Foods.ToList();
            }
        }
    }
}
