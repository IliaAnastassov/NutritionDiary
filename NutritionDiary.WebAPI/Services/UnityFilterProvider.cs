using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Unity;

namespace NutritionDiary.WebAPI.Services
{
    public class UnityFilterProvider : IFilterProvider
    {
        private IUnityContainer _container;

        public UnityFilterProvider(IUnityContainer container)
        {
            _container = container;
        }

        public IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            var controllerFilters = actionDescriptor.ControllerDescriptor
                                                    .GetFilters()
                                                    .Select(instance => new FilterInfo(instance, FilterScope.Controller));

            var actionFilters = actionDescriptor.GetFilters()
                                                .Select(instance => new FilterInfo(instance, FilterScope.Action));

            var filters = controllerFilters.Concat(actionFilters);

            foreach (var filter in filters)
            {
                _container.BuildUp(filter.Instance.GetType(), filter.Instance);
            }

            return filters;
        }
    }
}