using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.WebAPI.Models;
using NutritionDiary.WebAPI.Services;

namespace NutritionDiary.WebAPI.Controllers
{
    public class DiariesController : BaseApiController
    {
        private INutritionDiaryIdentityService _identityService;

        public DiariesController(INutritionDiaryRepository repository, INutritionDiaryIdentityService identityService)
            : base(repository)
        {
            _identityService = identityService;
        }

        public IHttpActionResult Get()
        {
            var diaries = Repository.GetDiaries(_identityService.CurrentUser)
                                    .ToList();

            if (!diaries.Any())
            {
                return NotFound();
            }

            var models = diaries.Select(d => ModelFactory.Create(d));
            return Ok(models);
        }

        public IHttpActionResult Get(DateTime diaryId)
        {
            var diary = Repository.GetDiary(_identityService.CurrentUser, diaryId);

            if (diary == null)
            {
                return NotFound();
            }

            var model = ModelFactory.Create(diary);
            return Ok(model);
        }
    }
}
