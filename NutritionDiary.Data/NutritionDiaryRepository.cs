using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
                return db.Foods
                         .Include(f => f.Measures)
                         .ToList();
            }
        }
    }
}
