using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace UnicefEducationMIS.Core.AppConstants
{
    public class HiddenPermissions
    {
        public const string HaveToBeLoggedIn = "HaveToBeLoggedIn";

        public static List<string> All()
        {
            Type t = typeof(HiddenPermissions);

            return t.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
            .Select(x => x.GetValue(null).ToString())
            .ToList();
        }
    }
}
