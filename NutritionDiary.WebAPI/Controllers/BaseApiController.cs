using System.Web.Http;
using NutritionDiary.Data.Interfaces;
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
                    _modelFactory = new ModelFactory(Request);
                }

                return _modelFactory;
            }
        }
    }
}
