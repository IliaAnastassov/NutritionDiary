using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NutritionDiary.WebAPI.Utilities
{
    public static class Constants
    {
        public const string DEFAULT_CONNECTION_KEY = "DefaultConnection";
        public const string SELF_REL = "self";
        public const string NEW_DIARY_ENTRY_REL = "newDiaryEntry";
        public const string PREV_PAGE_REL = "prevPage";
        public const string NEXT_PAGE_REL = "nextPage";
        public const string FOODS_ROUTE = "Foods";
        public const string MEASURES_ROUTE = "Measures";
        public const string DIARIES_ROUTE = "Diaries";
        public const string DIARY_ENTRIES_ROUTE = "DiaryEntries";
        public const string DIARY_SUMMARY_ROUTE = "DiarySummary";
        public const string TOKEN_ROUTE = "Token";
        public const string DIARY_DATE_FORMAT = "yyyy-MM-dd";
        public const string GET = "GET";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string DELETE = "DELETE";
    }
}