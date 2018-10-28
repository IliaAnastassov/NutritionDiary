using System;
using System.Linq;
using NutritionDiary.Entities;

namespace NutritionDiary.Data.Interfaces
{
    public interface INutritionDiaryRepository
    {
        Food GetFood(int id);
        IQueryable<Food> GetAllFoods();
        IQueryable<Food> GetAllFoodsWithMeasures();
        IQueryable<Measure> GetMeasuresForFood(int foodId);
        Measure GetMeasure(int measureId);
        IQueryable<Diary> GetDiaries(string username);
        Diary GetDiary(string username, DateTime diaryId);
        IQueryable<DiaryEntry> GetDiaryEntries(string username, DateTime diaryId);
        DiaryEntry GetDiaryEntry(string username, DateTime diaryId, int entryId);
        bool DeleteDiaryEntry(int id);
        bool Commit();
    }
}
