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
            var username = _identityService.CurrentUser;
            var diaries = Repository.GetDiaries(username)
                                    .ToList();

            if (diaries == null)
            {
                return NotFound();
            }

            var models = diaries.Select(d => ModelFactory.Create(d));
            return Ok(models);
        }

        public IHttpActionResult Get(DateTime diaryId)
        {
            var username = _identityService.CurrentUser;
            var diary = Repository.GetDiary(username, diaryId);

            if (diary == null)
            {
                return NotFound();
            }

            var model = ModelFactory.Create(diary);
            return Ok(model);
        }
    }
}
