using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var type = value.GetType();
            
            var name = Enum.GetName(type, value);
            if (name == null) return null;
            
            var field = type.GetField(name);
            if (field == null) return null;

            if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attr)
            {
                return attr.Description;
            }
            return null;
        }

        public static bool NoneMatched(this Enum value)
        {
            return value.ToString() == "0";
        }
    }
}
