using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.Entities;
using NutritionDiary.WebAPI.Models;
using NutritionDiary.WebAPI.Services;

namespace NutritionDiary.WebAPI.Controllers
{
    public class DiaryEntriesController : BaseApiController
    {
        private INutritionDiaryIdentityService _identityService;

        public DiaryEntriesController(INutritionDiaryRepository repository, INutritionDiaryIdentityService identityService)
            : base(repository)
        {
            _identityService = identityService;
        }

        public IHttpActionResult Get(DateTime diaryId)
        {
            var diaryEntries = Repository.GetDiaryEntries(diaryId)
                                         .ToList();

            if (!diaryEntries.Any())
            {
                return NotFound();
            }

            var models = diaryEntries.Select(e => ModelFactory.Create(e));
            return Ok(models);
        }

        public IHttpActionResult Get(DateTime diaryId, int entryId)
        {
            var diaryEntry = Repository.GetDiaryEntry(diaryId, entryId);

            if (diaryEntry == null)
            {
                return NotFound();
            }

            var model = ModelFactory.Create(diaryEntry);
            return Ok(model);
        }
    }
}
