﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.Entities;
using static NutritionDiary.WebAPI.Utilities.Constants;

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
                Description = food.Description,
                Measures = food.Measures.Select(m => Create(m)),
                Links = new List<LinkModel>
                {
                    new LinkModel
                    {
                        Href = _urlHelper.Link("Foods", new { foodid = food.Id }),
                        Rel = SELF_REL
                    }
                }
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
            var selfHref = _urlHelper.Link("Diaries", new { diaryid = diary.CurrentDate.ToString("yyyy-MM-dd") });
            var createDiaryEntryHref = _urlHelper.Link("DiaryEntries", new { diaryid = diary.CurrentDate.ToString("yyyy-MM-dd") });

            var model = new DiaryModel
            {
                Links = new List<LinkModel>
                {
                    CreateLink(selfHref, SELF_REL),
                    CreateLink(createDiaryEntryHref, "newDiaryEntry", "POST")
                },
                CurrentDate = diary.CurrentDate,
                DiaryEntries = diary.Entries.Select(e => Create(e))
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

        internal AuthTokenModel Create(AuthToken authToken)
        {
            var model = new AuthTokenModel
            {
                Token = authToken.Token,
                Expiration = authToken.Expiration
            };

            return model;
        }

        internal MeasureV2Model CreateVersion2(Measure measure)
        {
            var model = new MeasureV2Model
            {
                Url = _urlHelper.Link("Measures", new { foodid = measure.Food.Id, measureid = measure.Id }),
                Description = measure.Description,
                Calories = measure.Calories,
                TotalFat = measure.TotalFat,
                Protein = measure.Protein,
                Carbohydrates = measure.Carbohydrates,
                Sugar = measure.Sugar,
                Sodium = measure.Sodium,
                Iron = measure.Iron,
                Cholestrol = measure.Cholestrol
            };

            return model;
        }

        internal object CreateDiarySummary(Diary diary)
        {
            var model = new DiarySummaryModel
            {
                DiaryDate = diary.CurrentDate,
                TotalCalories = Convert.ToInt32(diary.Entries.Sum(e => e.Measure.Calories * e.Quantity))
            };

            return model;
        }

        public LinkModel CreateLink(string href, string rel, string method = "GET", bool isTemplated = false)
        {
            var link = new LinkModel
            {
                Href = href,
                Rel = rel,
                Method = method,
                IsTemplated = isTemplated
            };

            return link;
        }

        public Diary Parse(DiaryModel model)
        {
            try
            {
                var entity = new Diary();

                var selfLink = model.Links.FirstOrDefault(l => l.Rel == SELF_REL);
                if (selfLink != null && !string.IsNullOrWhiteSpace(selfLink.Href))
                {
                    var uri = new Uri(selfLink.Href);
                    entity.Id = int.Parse(uri.Segments.Last());
                }

                entity.CurrentDate = model.CurrentDate;

                if (model.DiaryEntries != null)
                {
                    foreach (var entryModel in model.DiaryEntries)
                    {
                        var entry = Parse(entryModel);
                        entity.Entries.Add(entry);
                    }
                }

                return entity;
            }
            catch
            {
                return null;
            }
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

                if (!string.IsNullOrWhiteSpace(model.MeasureUrl))
                {
                    var uri = new Uri(model.MeasureUrl);
                    var measureId = int.Parse(uri.Segments.Last());
                    var measure = _repository.GetMeasure(measureId);
                    entity.Measure = measure;
                    entity.FoodItem = measure.Food;
                }

                return entity;
            }
            catch
            {
                return null;
            }
        }
    }
}
