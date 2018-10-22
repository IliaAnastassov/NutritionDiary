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
            var food = _db.Foods
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

        public IQueryable<Measure> GetMeasuresForFood(int foodId)
        {
            return _db.Measures
                      .Include(m => m.Food)
                      .Where(m => m.Food.Id == foodId);
        }

        public Measure GetMeasure(int measureId)
        {
            var measure = _db.Measures
                             .Include(m => m.Food)
                             .Where(m => m.Id == measureId)
                             .FirstOrDefault();

            if (measure == null)
            {
                measure = new Measure();
            }

            return measure;
        }
    }
}
