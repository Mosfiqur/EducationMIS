using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnicefEducationMIS.Core.Import;

namespace UnicefEducationMIS.Service.Import
{
    public class FacilityExcelColumns
    {
        public const string Id = "Id";
        public const string UpazilaId = "Upazila";
        public const string UnionId = "Union";
        public const string ParaName = "Para Name";
        public const string ParaId = "Para ID";
        public const string FacilityId = "Facility ID";
        public const string FacilityName = "Facility Name";
        public const string TargetedPopulation = "Targeted Population";        
        public const string CampSsId = "Camp SSID";
        public const string BlockId = "Block ID";

        public const string FacilityType = "Facility Type";
        public const string FacilityStatus = "Facility Status";

        public const string Latitude = "Latitude";
        public const string Longitude = "Longitude";
        public const string Donors = "Donors";
        public const string ProgramPartner = "Program Partner";
        public const string ImplementationPartner = "Implementation Partner";

        public const string NonEducationPartner = "Non Education Partner";
        public const string Remarks = "Remarks";


        public const int FacilityIdIndex = 0;

        public static List<ExcelCell> All()
        {
            return new List<ExcelCell>()
            {
                new FacilityExcelColumn(Id.ToLower(), Id),
                new FacilityExcelColumn(UpazilaId.ToLower(), "UpazilaId", true),
                new FacilityExcelColumn(UnionId.ToLower(), "UnionId", true),
                new FacilityExcelColumn(ParaName.ToLower(), "ParaName"),
                new FacilityExcelColumn(ParaId.ToLower(), "ParaId"),
                new FacilityExcelColumn(FacilityId.ToLower(), "FacilityId", true),
                new FacilityExcelColumn(FacilityName.ToLower(), "FacilityName", true),
                new FacilityExcelColumn(TargetedPopulation.ToLower(), "TargetedPopulation", true),
                new FacilityExcelColumn(CampSsId.ToLower(), "CampSsId"),
                new FacilityExcelColumn(BlockId.ToLower(), "BlockId"),
                new FacilityExcelColumn(FacilityType.ToLower(), "FacilityType"),
                new FacilityExcelColumn(FacilityStatus.ToLower(), "FacilityStatus"),
                new FacilityExcelColumn(Latitude.ToLower(), "Latitude"),
                new FacilityExcelColumn(Longitude.ToLower(), "Longitude"),
                new FacilityExcelColumn(Donors.ToLower(), "Donors"),
                new FacilityExcelColumn(ProgramPartner.ToLower(), "ProgramPartner", true),
                new FacilityExcelColumn(ImplementationPartner.ToLower(), "ImplementationPartner", true),
                new FacilityExcelColumn(NonEducationPartner.ToLower(), "NonEducationPartner"),
                new FacilityExcelColumn(Remarks.ToLower(), "Remarks"),
            };
        }

        
        public static List<ExcelCell> VersionedMandatoryColumns()
        {
            return new List<ExcelCell>()
            {
                new FacilityExcelColumn(Id, Id, false),
                new FacilityExcelColumn(FacilityId.ToLower(), "FacilityId", false),
                new FacilityExcelColumn(FacilityName.ToLower(), "FacilityName", true)
            };
        }
    }
}
