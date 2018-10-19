using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace NutritionDiary.Data.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<NutritionDiaryDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NutritionDiaryDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
