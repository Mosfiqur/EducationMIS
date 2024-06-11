using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.ViewModel.Dashboard;

namespace UnicefEducationMIS.Core.Extensions
{
    public static class ObjectExtension
    {
        public const string DateTimeWithoutSecondFormat = "dd-MMM-yyyy";
        public static string ToStringValue(this object value)
        {
            if (value == null)
                return String.Empty;
            var type = value.GetType();
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                var dateTime = (DateTime)value;
                if (dateTime != DateTime.MinValue)
                    return dateTime.ToString(DateTimeWithoutSecondFormat);
            }

            if (type.IsEnum)
                return ((int)value).ToString();
            var s = Convert.ToString(value, CultureInfo.InvariantCulture);
            return string.IsNullOrEmpty(s) ? s : s.Trim();
        }

        public static bool IsEmpty(this List<RowError> rowErrors)
        {
            return rowErrors.Count == 0;
        }

        public static string ToHexString(this Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

        public static void AddGapColorGradients(this List<GapMapViewModel> gapmaps)
        {
            var start = ColorTranslator.FromHtml("#296275");
            var end = ColorTranslator.FromHtml("#297559");
            var zero = ColorTranslator.FromHtml("#b0c8d0");


            var positive = gapmaps.Select(x => x.Gap)
                .Where(x => x > 0)
                .Distinct()
                .OrderByDescending(x => x)
                .ToList();

            var negative = gapmaps.Select(x => x.Gap)
                .Where(x => x < 0)
                .Distinct()
                .OrderByDescending(x => x)
                .ToList();

            var colorDict = new Dictionary<string, string>();
            colorDict.Add("0", "#b0c8d0");

            if(positive.Count > 1)
            {
                var positiveGrad = GetGradients(start, zero, positive.Count).GetEnumerator();
                positive.ForEach(gap =>
                {
                    positiveGrad.MoveNext();
                    colorDict.Add(gap.ToString(), positiveGrad.Current.ToHexString());
                });
            }
            else if(positive.Count == 1)
            {
                positive.ForEach(gap =>
                {                    
                    colorDict.Add(gap.ToString(), start.ToHexString());
                });
            }

            if(negative.Count > 1)
            {
                var negativeGrad = GetGradients(zero, end, negative.Count).GetEnumerator();
                negative.ForEach(gap =>
                {
                    negativeGrad.MoveNext();
                    colorDict.Add(gap.ToString(), negativeGrad.Current.ToHexString());
                });
            }
            else if(negative.Count == 1)
            {
                negative.ForEach(gap =>
                {                    
                    colorDict.Add(gap.ToString(), end.ToHexString());
                });
            }
            
            gapmaps.ForEach(x => {
                colorDict.TryGetValue(x.Gap.ToString(), out var value);
                x.FillColor = value;
            });
        }

        public static IEnumerable<Color> GetGradients(Color start, Color end, int steps)
        {
            int stepA = ((end.A - start.A) / (steps - 1));
            int stepR = ((end.R - start.R) / (steps - 1));
            int stepG = ((end.G - start.G) / (steps - 1));
            int stepB = ((end.B - start.B) / (steps - 1));

            for (int i = 0; i < steps; i++)
            {
                yield return Color.FromArgb(start.A + (stepA * i),
                                            start.R + (stepR * i),
                                            start.G + (stepG * i),
                                            start.B + (stepB * i));
            }
        }

        public static List<string> AsList(this String str)
        {
            return new List<string>() {str};
        }

        public static List<string> ToMultiValue(this String str)
        {
            return str.Split(',').ToList();
        }

        public static T TryParseToEnum<T>(this String str, out bool result) where T : Enum
        {
            var val = Enum.GetValues(typeof(T))
                .Cast<T>()
                .FirstOrDefault(x => x.ToString() == str || x.GetDescription() == str);
            result = val?.ToString() != "0";
            return val;
        }

    }
}
