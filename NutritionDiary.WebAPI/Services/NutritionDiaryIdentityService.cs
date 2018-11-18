using System.Threading;

namespace NutritionDiary.WebAPI.Services
{
    public class NutritionDiaryIdentityService : INutritionDiaryIdentityService
    {
        public string CurrentUser
        {
            get
            {
#if !DEBUG
                return Thread.CurrentPrincipal.Identity.Name;
#endif
                return "ianastassov";
            }
        }
    }
}