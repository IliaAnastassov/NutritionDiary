using System;

namespace NutritionDiary.WebAPI.Models
{
    public class AuthTokenModel
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}