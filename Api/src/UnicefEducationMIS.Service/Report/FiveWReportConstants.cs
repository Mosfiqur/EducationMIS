using System.Collections.Generic;

namespace UnicefEducationMIS.Service.Report
{
    public class FiveWReportConstants
    {
        public const string ColUpazilla = "Upazila";
        public const string ColUnion = "Union";
        public const string ColCamp = "Camp";
        public const string ColCampSSID = "Camp SSID";
        public const string ColParaName = "Para Name";
        public const string ColParaID = "Para ID";
        public const string ColBlockName = "Block Name";
        public const string ColFacilityName = "Facility Name";
        public const string ColFacilityID = "Facility ID";
        public const string ColTargetedPopulation = "Targeted Population";
        public const string ColType = "Type";
        public const string ColFacilityStatus = "Facility Status";
        public const string ColLatitude = "Latitude";
        public const string ColLongitude = "Longitude";
        public const string ColDonors = "Donors";
        public const string ColProgrammePartner = "Programme Partner";
        public const string ColImplementingPartners = "Implementing Partners";
        public const string ColNonEducationPartners = "Non Education Partners";

        public static readonly List<string> FixedColumns = new List<string>
        {
            ColUpazilla, ColUnion, ColCamp, ColCampSSID, ColParaName, ColParaID,
            ColBlockName, ColFacilityName, ColFacilityID, ColTargetedPopulation, ColType,
            ColFacilityStatus, ColLatitude, ColLongitude, ColDonors, ColProgrammePartner,
            ColImplementingPartners, ColNonEducationPartners
        };

        public static readonly Dictionary<int, string> IndexToColumns = new Dictionary<int, string>
        {
            {0, ColUpazilla}, {1, ColUnion}, {2, ColCamp}, {3, ColCampSSID}, {4, ColParaName}, {5, ColParaID},
            {6, ColBlockName}, {7, ColFacilityName}, {8, ColFacilityID}, {9, ColTargetedPopulation}, {10, ColType}, {11, ColFacilityStatus},
            {12, ColLatitude}, {13, ColLongitude}, {14, ColDonors}, {15, ColProgrammePartner}, {16, ColImplementingPartners}, {17, ColNonEducationPartners}
        };

        public static readonly List<long> Indicators = new List<long>()
        {

            19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,41,42,43,44,45,46
                    ,47,48,49,50,51,52,53,54,55,56,57,58,59,60,61,62,63,64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79
                    ,80,81,82,83,84,85,86,87,88,89,90,91,92,93,94,95,96,97,98,99,100,101,102
                    ,103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,119,120,121
        };

        public const string ColReportingPersonName = "Reporting person's name";
        public const string ColReportingPersonEmail = "Reporting person's email";
        public const string ColReportingPersonPhone = "Reporting person's phone";
        public const string ColFacilityLastUpdatedDate = "Facility information updated till date";
        public static readonly List<string> ReportingPersonInfo = new List<string>
        {
            ColReportingPersonName,ColReportingPersonEmail,ColReportingPersonPhone,ColFacilityLastUpdatedDate
        };

        public static readonly Dictionary<int, string> IndexToReportingPerson = new Dictionary<int, string>
        {
            {0, ColReportingPersonName}, {1, ColReportingPersonEmail}, {2, ColReportingPersonPhone},{3,ColFacilityLastUpdatedDate}
        };

        public const string ColAllRohingyaGirls = "All rohingya Girls";
        public const string ColAllRohingyaBoys = "All rohingya Boys";
        public const string ColAllHostGirls = "All host Girls";
        public const string ColAllHostBoys = "All host Boys";
        public const string ColAllEnrolments = "All Enrollments";
        public const string ColTeachers = "Teachers";
        public const string ColPRogramAndImplementaion = "Programme and implementing";
        public static readonly List<string> CalculatedField = new List<string>
        {
            ColAllRohingyaGirls,ColAllRohingyaBoys ,ColAllHostGirls,ColAllHostBoys,ColAllEnrolments,ColTeachers,ColPRogramAndImplementaion
        };
        public static readonly Dictionary<int, string> IndexToCalculatedField = new Dictionary<int, string>
        {
            {0, ColAllRohingyaGirls}, {1, ColAllRohingyaBoys}, {2, ColAllHostGirls}, {3, ColAllHostBoys}, {4, ColAllEnrolments}, {5, ColTeachers},
            {6, ColPRogramAndImplementaion}
        };
        public static readonly List<long> AllRohingyaGirlsIndicator = new List<long>()
        {
            35,47, 59,71,83
        };
        public static readonly List<long> AllRohingyaBoysIndicator = new List<long>()
        {
            38,50,62,74,86
        };
        public static readonly List<long> AllHostGirlsIndicator = new List<long>()
        {
            41,53,65,77,89
        };
        public static readonly List<long> AllHostBoysIndicator = new List<long>()
        {
            44,56,68,80,92
        };
        public static readonly List<long> AllEnrollmentIndicator = new List<long>()
        {
            35,47, 59,71,83,38,50,62,74,86,41,53,65,77,89,44,56,68,80,92
        };

        public static readonly List<long> AllTeachers = new List<long>()
        {
           95,96,97,98
        };

    }
}
