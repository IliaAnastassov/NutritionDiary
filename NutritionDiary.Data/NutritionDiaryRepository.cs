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

        public Diary GetDiary(string username, DateTime diaryId)
        {
            var diary = _db.Diaries
                           .Include(d => d.Entries)
                           .Include(d => d.Entries.Select(e => e.FoodItem))
                           .Include(d => d.Entries.Select(e => e.Measure))
                           .Where(d => d.UserName == username && d.CurrentDate == diaryId)
                           .FirstOrDefault();

            return diary;
        }

        public IQueryable<DiaryEntry> GetDiaryEntries(string username, DateTime diaryId)
        {
            var diaryEntries = _db.DiaryEntries
                                  .Include(e => e.Diary)
                                  .Include(e => e.FoodItem)
                                  .Include(e => e.Measure)
                                  .Where(e => e.Diary.UserName == username && e.Diary.CurrentDate == diaryId);

            return diaryEntries;
        }

        public DiaryEntry GetDiaryEntry(DateTime diaryId, int entryId)
        {
            var diaryEntry = _db.DiaryEntries
                                .Include(e => e.Diary)
                                .Include(e => e.FoodItem)
                                .Include(e => e.Measure)
                                .Where(e => e.Diary.CurrentDate == diaryId && e.Id == entryId)
                                .FirstOrDefault();

            return diaryEntry;
        }

        public bool DeleteDiaryEntry(int id)
        {
            try
            {
                var diaryEntry = _db.DiaryEntries.Find(id);
                _db.DiaryEntries.Remove(diaryEntry);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Commit()
        {
            return _db.SaveChanges() > 0;
        }
    }
}
