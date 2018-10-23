﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Routing;
using NutritionDiary.Entities;

namespace NutritionDiary.WebAPI.Models
{
    public class ModelFactory
    {
        private UrlHelper _urlHelper;

        public ModelFactory(HttpRequestMessage request)
        {
            _urlHelper = new UrlHelper(request);
        }

        public FoodModel Create(Food food)
        {
            var model = new FoodModel
            {
                Url = _urlHelper.Link("Foods", new { foodid = food.Id }),
                Description = food.Description,
                Measures = food.Measures.Select(m => Create(m))
            };

            return model;
        }

        public MeasureModel Create(Measure measure)
        {
            var model = new MeasureModel
            {
                Url = _urlHelper.Link("Measures", new { foodid = measure.Food.Id, measureid = measure.Id }),
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