using System.Collections.Generic;

namespace UnicefEducationMIS.Service.Report
{
    public class ImportTemplateConstants
    {
        public const string ColId = "Id"; 
        public const string ColUnhcrId = "proGres ID"; 
        public const string ColBeneficiaryName = "Beneficiary Name"; 
        public const string ColFacilityId = "Facility Id"; 
        public const string ColFacilityName = "Facility Name"; 
        public const string TargetedPopulation = "Targeted Population"; 
        public const string Status = "Facility Status"; 
        public const string Latitude = "Latitude"; 
        public const string Longitude = "Longitude"; 
        public const string Donors = "Donors"; 
        public const string NonEducationPartner = "Non Education Partner"; 
        public const string ProgramPartner = "Program Partner"; 
        public const string ImplementationPartner = "Implementation Partner"; 
        public const string Remarks = "Remarks"; 
        public const string Upazila = "Upazila"; 
        public const string Union = "Union"; 
        public const string Camp = "Camp"; 
        public const string ParaName = "Para Name"; 
        public const string Block = "Block"; 
        public const string Teacher = "Teacher"; 
        public const string Type = "Facility Type";

        // Beneficiary columns
        public const string FatherName = "Father Name";
        public const string MotherName = "Mother Name";
        public const string FCNId = "FCN Id";
        public const string DateOfBirth = "Date Of Birth";
        public const string Sex = "Sex";
        public const string Disabled = "Disabled";
        public const string EducationLevel = "Education level";
        public const string EnrollmentDate = "Enrollment Date";
        public const string SubBlock = "Sub Block";
        

        public static readonly List<string> FixedColumnsForBeneficiaryVersionData = new List<string>
        {
            ColId,
            ColFacilityId, 
            ColFacilityName,
            ColUnhcrId,
            ColBeneficiaryName,
            FatherName,
            MotherName,
            FCNId,
            DateOfBirth,
            Sex,
            Disabled,
            EducationLevel,
            EnrollmentDate,
            Camp,
            Block,
            SubBlock,
            Remarks

        };


        public static readonly List<string> FixedColumnsForFacilityVersionData = new List<string>
        {
            ColId, 
            ColFacilityId, 
            ColFacilityName,
            TargetedPopulation,
            Status,
            Latitude,
            Longitude,
            Donors,
            NonEducationPartner,
            ProgramPartner,
            ImplementationPartner,
            Remarks,
            Upazila,
            Union,
            Camp,
            ParaName,
            Block,
            Teacher,
            Type

        };
    }
}
