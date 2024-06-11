using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class JrpDashboardBeneficairyRawData
    {
        public DateTime DateOfBirth { get; set; }
        public Gender Sex { get; set; }
        public bool Disabled { get; set; }
        public LevelOfStudy LevelOfStudy { get; set; }
        public int BeneficiaryCampId { get; set; }

        public string Value { get; set; }

        public int Age
        {
            get
            {
                DateTime now = DateTime.Now;
                int age = now.Year - DateOfBirth.Year;
                if (now.Month < DateOfBirth.Month || (now.Month == DateOfBirth.Month && now.Day < DateOfBirth.Day))
                    age--;
                return age;
            }
        }


    }
}
