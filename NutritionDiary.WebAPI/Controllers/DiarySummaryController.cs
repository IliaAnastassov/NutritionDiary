using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.WebAPI.Services;

namespace NutritionDiary.WebAPI.Controllers
{
    public class DiarySummaryController : BaseApiController
    {
        private INutritionDiaryIdentityService _identityService;

        public DiarySummaryController(INutritionDiaryRepository repository, INutritionDiaryIdentityService identityService)
            : base(repository)
        {
            _identityService = identityService;
        }

        public IHttpActionResult Get(DateTime diaryId)
        {
            try
            {
                var diary = Repository.GetDiary(_identityService.CurrentUser, diaryId);
                var model = ModelFactory.CreateDiarySummary(diary);

                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
