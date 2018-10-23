using System;
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

            return measure;
        }

        public IQueryable<Diary> GetDiaries(string username)
        {
            return _db.Diaries
                      .Where(d => d.UserName == username);
        }

        public Diary GetDiary(DateTime diaryId)
        {
            var diary = _db.Diaries
                           .Include(d => d.Entries)
                           .Where(d => d.CurrentDate == diaryId)
                           .FirstOrDefault();

            return diary;
        }
    }
}
