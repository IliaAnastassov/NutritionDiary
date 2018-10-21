using System.Collections.Generic;

namespace NutritionDiary.Entities
{
    public class Food
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Measure> Measures { get; set; } = new List<Measure>();
    }
}
