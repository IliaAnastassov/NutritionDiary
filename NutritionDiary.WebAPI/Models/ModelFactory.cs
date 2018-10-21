using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NutritionDiary.Entities;

namespace NutritionDiary.WebAPI.Models
{
    public class ModelFactory
    {
        public FoodModel Create(Food food)
        {
            var model = new FoodModel
            {
                Description = food.Description,
                Measures = food.Measures.Select(m => Create(m))
            };

            return model;
        }

        public MeasureModel Create(Measure measure)
        {
            var model = new MeasureModel
            {
                Description = measure.Description,
                Calories = measure.Calories,
                TotalFat = measure.TotalFat,
                Protein = measure.Protein,
                Carbohydrates = measure.Carbohydrates,
                Sugar = measure.Sugar
            };

            return model;
        }
    }
}