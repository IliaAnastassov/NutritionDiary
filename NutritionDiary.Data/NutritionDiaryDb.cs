using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NutritionDiary.Entities;

namespace NutritionDiary.Data
{
    internal class NutritionDiaryDb : DbContext
    {
        public DbSet<Food> Foods { get; set; }
        public DbSet<Measure> Measures { get; set; }
    }
}
