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
        Diary GetDiary(DateTime diaryId);
    }
}
