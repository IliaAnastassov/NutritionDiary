using System;
using System.Collections.Generic;

namespace NutritionDiary.Entities
{
    public class Diary
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime CurrentDate { get; set; }

        public virtual ICollection<DiaryEntry> Entries { get; set; } = new List<DiaryEntry>();
    }
}
