using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Routing;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.Entities;
using NutritionDiary.WebAPI.Models;
using static NutritionDiary.WebAPI.Utilities.Constants;

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
            try
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
                var pageCount = Convert.ToInt32(Math.Ceiling((double)totalCount / PAGE_SIZE));

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
                        Links = GetLinks(page, pageCount),
                        Results = models
                    }
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IHttpActionResult Get(int foodId)
        {
            try
            {
                var food = Repository.GetFood(foodId);

                if (food == null)
                {
                    return NotFound();
                }

                var model = ModelFactory.Create(food);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private IEnumerable<LinkModel> GetLinks(int currentPage, int pageCount)
        {
            var links = new List<LinkModel>();

            var prevPageUrl = GetPrevPageUrl(currentPage, pageCount);
            if (!string.IsNullOrEmpty(prevPageUrl))
            {
                var prevPageLink = ModelFactory.CreateLink(prevPageUrl, PREV_PAGE_REL);
                links.Add(prevPageLink);
            }

            var nextPageUrl = GetNextPageUrl(currentPage, pageCount);
            if (!string.IsNullOrEmpty(nextPageUrl))
            {
                var nextPageLink = ModelFactory.CreateLink(nextPageUrl, NEXT_PAGE_REL);
                links.Add(nextPageLink);
            }

            return links;
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
            return helper.Link(FOODS_ROUTE, new { page = pageNumber });
        }
    }
}
