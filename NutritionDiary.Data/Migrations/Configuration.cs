using System;
using System.Collections.Generic;
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
            AutomaticMigrationDataLossAllowed = true;
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
                new Food
                {
                    Description = "Apple",
                    Measures = new List<Measure>
                    {
                        new Measure
                        {
                            Description = "1 fruit, medium sized",
                            Calories = 150,
                            Protein = 5,
                            Carbohydrates = 60,
                            Sugar = 30,
                            TotalFat = 5,
                            Fiber = 12
                        }
                    }
                },
                new Food
                {
                    Description = "Bannana",
                    Measures = new List<Measure>
                    {
                        new Measure
                        {
                            Description = "1 fruit, large sized",
                            Calories = 200,
                            Protein = 10,
                            Carbohydrates = 65,
                            Sugar = 20,
                            TotalFat = 12,
                            Fiber = 15
                        }
                    }
                }
            );
        }
    }
}
