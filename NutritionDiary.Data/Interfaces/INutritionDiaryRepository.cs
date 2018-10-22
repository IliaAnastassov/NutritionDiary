using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
