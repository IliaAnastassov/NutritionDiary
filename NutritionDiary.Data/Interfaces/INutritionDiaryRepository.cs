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
        IEnumerable<Food> GetAllFoods();
        Food GetFood(int id);
    }
}
