using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.Entities;

namespace NutritionDiary.Data
{
    public class NutritionDiaryRepository : INutritionDiaryRepository
    {
        private NutritionDiaryDb _db;

        public NutritionDiaryRepository(NutritionDiaryDb db)
        {
            _db = db;
        }

        public Food GetFood(int id)
        {
            var food =  _db.Foods
                           .Include(f => f.Measures)
                           .Where(f => f.Id == id)
                           .FirstOrDefault();

            if (food == null)
            {
                food = new Food();
            }

            return food;
        }

        public IQueryable<Food> GetAllFoods()
        {
            return _db.Foods;
        }

        public IQueryable<Food> GetAllFoodsWithMeasures()
        {
            return _db.Foods.Include(f => f.Measures);
        }
    }
}
