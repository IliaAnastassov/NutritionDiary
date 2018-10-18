using System.Data.Entity;
using NutritionDiary.Entities;

namespace NutritionDiary.Data
{
    internal class NutritionDiaryDb : DbContext
    {
        public NutritionDiaryDb() : base("DefaultConnection")
        {
        }

        public DbSet<ApiUser> ApiUsers { get; set; }
        public DbSet<AuthToken> AuthTokens { get; set; }
        public DbSet<Food> Foods { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<Diary> Diaries { get; set; }
        public DbSet<DiaryEntry> DiaryEntries { get; set; }
    }
}
