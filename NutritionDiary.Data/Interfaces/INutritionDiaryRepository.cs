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
        DiaryEntry GetDiaryEntry(string username, DateTime diaryId, int entryId);
        IQueryable<DiaryEntry> GetDiaryEntries(string username, DateTime diaryId);
        IQueryable<ApiUser> GetApiUsers();
        ApiUser GetApiUser(string apiKey);
        AuthToken GetAuthToken(string token);
        bool Insert(Diary diary);
        bool Insert(DiaryEntry diaryEntry);
        bool Insert(ApiUser apiUser);
        bool Insert(AuthToken authToken);
        bool DeleteDiaryEntry(int id);
        bool Commit();
    }
}
