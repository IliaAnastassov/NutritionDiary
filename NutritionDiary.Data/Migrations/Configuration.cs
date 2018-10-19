using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using NutritionDiary.Entities;

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
            context.Foods.AddOrUpdate(f => f.Description,
                new Food { Description = "Milk" },
                new Food { Description = "Butter" },
                new Food { Description = "Bread" },
                new Food { Description = "Steak" },
                new Food { Description = "Pizza" },
                new Food { Description = "Burger" },
                new Food { Description = "Rice" },
                new Food { Description = "Patatoe" },
                new Food { Description = "Apple" },
                new Food { Description = "Bannana" }
            );
        }
    }
}
