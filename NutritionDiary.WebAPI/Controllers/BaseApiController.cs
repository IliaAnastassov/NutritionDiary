﻿using System.Web.Http;
using NutritionDiary.Data.Interfaces;
using NutritionDiary.WebAPI.ActionResults;
using NutritionDiary.WebAPI.Models;

namespace NutritionDiary.WebAPI.Controllers
{
    public abstract class BaseApiController : ApiController
    {
        private INutritionDiaryRepository _repository;
        private ModelFactory _modelFactory;

        public BaseApiController(INutritionDiaryRepository repository)
        {
            _repository = repository;
        }

        protected INutritionDiaryRepository Repository
        {
            get
            {
                return _repository;
            }
        }

        protected ModelFactory ModelFactory
        {
            get
            {
                if (_modelFactory == null)
                {
                    _modelFactory = new ModelFactory(Request, Repository);
                }

                return _modelFactory;
            }
        }

        protected IHttpActionResult Versioned<T>(T body, string version = "v1")
            where T : class
        {
            return new VersionedActionResult<T>(Request, body, version);
        }
    }
}
