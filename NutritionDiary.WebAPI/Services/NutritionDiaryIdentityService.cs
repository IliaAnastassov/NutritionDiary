using System.Threading;

namespace NutritionDiary.WebAPI.Services
{
    public class NutritionDiaryIdentityService : INutritionDiaryIdentityService
    {
        public string CurrentUser
        {
            get
            {
                return Thread.CurrentPrincipal.Identity.Name;
            }
        }
    }
}