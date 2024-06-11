using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnicefEducationMIS.Core.AppConstants
{
    public class BeneficairyFixedColumns
    {
        public const long UnhcrId = 122;
        public const long Name = 123;
        public const long FatherName = 124;
        public const long MotherName = 125;
        public const long FCNId = 126;
        public const long DateOfBirth = 127;
        public const long Sex = 128;
        public const long Disabled = 129;
        public const long LevelOfStudy = 130;
        public const long EnrollmentDate = 131;
        public const long FacilityId = 132;
        public const long CampId = 133;
        public const long BlockId = 134;
        public const long SubBlockId = 135;
        public const long Remarks = 136;

        public const int Have_received_educational_material = 137;

        public static List<long> Basic()
        {
            Type t = typeof(BeneficairyFixedColumns);

            var res = t.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public)
                .Select(x => long.Parse(x.GetValue(null).ToString()))
                .Where(x => x != Have_received_educational_material)
                .ToList();
            return res;
        }

        public static List<long> Optional()
        {
            return new List<long>()
            {
                FatherName,MotherName,Remarks
            };
        }
    }
}
