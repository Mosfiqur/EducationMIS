using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.AppConstants
{
    public class FacilityFixedIndicators
    {
        public const long Code= 1;
        public const long Name= 2;
        public const long TargetedPopulation = 3;
        public const long Status= 4;
        public const long Latitude = 5;
        public const long Longitude = 6;
        public const long Donors = 7;
        public const long NonEducationPartner = 8;
        public const long ProgramPartner= 9;
        public const long ImplementationPartner= 10;
        public const long Remarks= 11;
        public const long Upazila = 12;
        public const long Union = 13;
        public const long Camp = 14;
        public const long ParaName = 15;
        public const long Block = 16;
        public const long Teacher = 17;
        public const long Type = 18;

        public static List<long> All()
        {
            Type t = typeof(FacilityFixedIndicators);

            return t.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Select(x => long.Parse(x.GetValue(null).ToString()))
                .ToList();
        }

        public static List<long> Optional()
        {
            return new List<long>()
            {
                Latitude,
                Longitude,
                Donors,
                NonEducationPartner,
                Remarks,
                ParaName,
                Teacher,
                Camp,
                Block,
                Status,
                Type
            };
        }

    }
}
