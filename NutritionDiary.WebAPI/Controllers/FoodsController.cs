using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.Entities;
using NutritionDiary.WebAPI.Models;

namespace NutritionDiary.WebAPI.Controllers
{
    public class FoodsController : BaseApiController
    {
        private const int PAGE_SIZE = 2;

        public FoodsController(INutritionDiaryRepository repository)
            : base(repository)
        {
        }

        public IHttpActionResult Get(bool includeMeasures = true, int page = 0)
        {
            IQueryable<Food> query;

            if (includeMeasures)
            {
                query = Repository.GetAllFoodsWithMeasures()
                                  .OrderBy(f => f.Description);
            }
            else
            {
                query = Repository.GetAllFoods()
                                  .OrderBy(f => f.Description);
            }

            var totalCount = query.Count();
            var pageCount = (int)Math.Ceiling((double)totalCount / PAGE_SIZE);

            var foods = query.Skip(PAGE_SIZE * page)
                             .Take(PAGE_SIZE)
                             .ToList();

            if (!foods.Any())
            {
                return NotFound();
            }

            var models = foods.Select(f => ModelFactory.Create(f));

            return Ok(
                new
                {
                    TotalCount = totalCount,
                    PageCount = pageCount,
                    PrevPageUrl = GetPrevPageUrl(page, pageCount),
                    NextPageUrl = GetNextPageUrl(page, pageCount),
                    Results = models
                }
            );
        }

        public IHttpActionResult Get(int foodId)
        {
            var food = Repository.GetFood(foodId);

            if (food == null)
            {
                return NotFound();
            }

            var model = ModelFactory.Create(food);
            return Ok(model);
        }

        private string GetPrevPageUrl(int currentPage, int pageCount)
        {
            var prevPageUrl = string.Empty;

            if (pageCount > 1 && currentPage > 0)
            {
                prevPageUrl = GetPageUrl(currentPage - 1);
            }

            return prevPageUrl;
        }

        private string GetNextPageUrl(int currentPage, int pageCount)
        {
            var nextPageUrl = string.Empty;

            if (pageCount > 1 && currentPage < pageCount - 1)
            {
                nextPageUrl = GetPageUrl(currentPage + 1);
            }

            return nextPageUrl;
        }

        private string GetPageUrl(int pageNumber)
        {
            var helper = new UrlHelper(Request);
            return helper.Link("Foods", new { page = pageNumber });
        }
    }
}
