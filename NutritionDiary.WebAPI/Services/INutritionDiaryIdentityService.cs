namespace NutritionDiary.WebAPI.Services
{
    public interface INutritionDiaryIdentityService
    {
        string CurrentUser { get; }
    }
}