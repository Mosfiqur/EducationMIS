using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace UnicefEducationMIS.Core.AppConstants
{
    public static class ValueConversions
    {
        public static Expression<Func<List<DayOfWeek>, string>> DayOfWeekListToString = list => string.Join(",", list);
        public static Expression<Func<string, List<DayOfWeek>>> StringToDayOfWeekList = str =>
            str
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(elem => (DayOfWeek)Enum.Parse(typeof(DayOfWeek), elem))
            .ToList();
    }
}
