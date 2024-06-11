using System.Collections.Generic;
using DocumentFormat.OpenXml.Office2010.ExcelAc;

namespace UnicefEducationMIS.Service.Import
{
    public class BeneficiaryExcelColumnNames
    {
        public const string UnhcrId = "proGres ID";
        public const string Name = "Name";
        public const string FatherName = "Father's Name";
        public const string MotherName = "Mother's Name";
        public const string FCNId = "FCN_Id";
        public const string DateOfBirth = "Date of Birth";
        public const string Sex = "Sex";
        public const string Disabled = "Disabled";
        public const string LevelOfStudy = "Level of Study";
        public const string EnrollmentDate = "Enrollment Date of Current Level";
        public const string FacilityId = "Facility ID";   
        public const string CampSSId = "Camp SSID";  
        public const string BlockId = "Block ID";  
        public const string SubBlockId = "Sub Block ID";  


        public static readonly List<string> MandatoryColumns = new List<string>
        {
            UnhcrId, Name, DateOfBirth 
        };

        public static readonly List<string> AllColumns = new List<string>
        {
            UnhcrId, Name, FatherName, MotherName, FCNId, DateOfBirth, Sex, Disabled, LevelOfStudy, EnrollmentDate, FacilityId, CampSSId, BlockId, SubBlockId
        };

    }
}
