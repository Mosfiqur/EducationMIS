using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Common;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Extensions
{
    public static class ListExtensions
    {
        public static string Second(this List<string> source)
        {
            return source[1];
        }

        public static string SecondLast(this List<string> source)
        {
            return source[source.Count - 2];
        }     

        public static bool Contains(this List<DateRange> dateRanges, DateTime date)
        {
            return dateRanges.Any(x => x.From <= date && x.To >= date);
        }

        public static bool IsPointInside(this List<Coordinate> polygon, Coordinate testPoint)
        {
            bool result = false;
            int j = polygon.Count() - 1;
            for (int i = 0; i < polygon.Count(); i++)
            {
                if (polygon[i].Latitude < testPoint.Latitude && polygon[j].Latitude >= testPoint.Latitude || polygon[j].Latitude < testPoint.Latitude && polygon[i].Latitude >= testPoint.Latitude)
                {
                    if (polygon[i].Longitude + (testPoint.Latitude - polygon[i].Latitude) / (polygon[j].Latitude - polygon[i].Latitude) * (polygon[j].Longitude - polygon[i].Longitude) < testPoint.Longitude)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public static bool IsPointInside(this List<CampCoordinate> polygon, Coordinate testPoint)
        {
            return polygon.Select(x => new Coordinate(x.Latitude, x.Longitude))
                .ToList().IsPointInside(testPoint);
        }

        public static T ToEnum<T>(this List<string> values)
        {
            return (T)Enum.Parse(typeof(T), values.FirstOrDefault() ?? string.Empty);
        }
    }
}
