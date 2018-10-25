using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.Entities;

namespace NutritionDiary.WebAPI.Models
{
    public class ModelFactory
    {
        private UrlHelper _urlHelper;
        private INutritionDiaryRepository _repository;

        public ModelFactory(HttpRequestMessage request, INutritionDiaryRepository repository)
        {
            _urlHelper = new UrlHelper(request);
            _repository = repository;
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

        public DiaryModel Create(Diary diary)
        {
            var model = new DiaryModel
            {
                Url = _urlHelper.Link("Diaries", new { diaryid = diary.CurrentDate.ToString("yyyy-MM-dd") }),
                CurrentDate = diary.CurrentDate
            };

            return model;
        }

        public DiaryEntryModel Create(DiaryEntry diaryEntry)
        {
            var model = new DiaryEntryModel
            {
                Url = _urlHelper.Link("DiaryEntries", new { diaryid = diaryEntry.Diary.CurrentDate.ToString("yyyy-MM-dd"), entryid = diaryEntry.Id }),
                FoodDescription = diaryEntry.FoodItem.Description,
                MeasureDescription = diaryEntry.Measure.Description,
                MeasureUrl = _urlHelper.Link("Measures", new { foodid = diaryEntry.FoodItem.Id, measureid = diaryEntry.Measure.Id }),
                Quantity = diaryEntry.Quantity
            };

            return model;
        }

        public DiaryEntry Parse(DiaryEntryModel model)
        {
            try
            {
                var entity = new DiaryEntry();

                if (model.Quantity != default(double))
                {
                    entity.Quantity = model.Quantity;
                }

                var uri = new Uri(model.MeasureUrl);
                var measureId = int.Parse(uri.Segments.Last());
                var measure = _repository.GetMeasure(measureId);
                entity.Measure = measure;
                entity.FoodItem = measure.Food;

                return entity;
            }
            catch
            {
                return null;
            }
        }
    }
}