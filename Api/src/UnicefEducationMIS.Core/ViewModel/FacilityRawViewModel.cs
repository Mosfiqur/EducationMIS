using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
   public class FacilityRawViewModel
    {
       
        public long Id { get; set; }
        public string FacilityName { get; set; }
        public string FacilityCode { get; set; }

        public int UnionId { get; set; }
        public string UnionName { get; set; }
        public int UpazilaId { get; set; }
        public string UpazilaName { get; set; }
        public int? CampId { get; set; }
        public string CampName { get; set; }

        public string Donors { get; set; }
        public string NonEducationPartner { get; set; }
        public string Latitude { get; set; }
        public string longitude { get; set; }
        public string CampSSID { get; set; }

        public int? BlockId { get; set; }
        public string BlockName { get; set; }
        public string BlockCode { get; set; }
        public string ParaId { get; set; }
        public string ParaName { get; set; }

        public int ProgrammingPartnerId { get; set; }
        public string ProgrammingPartnerName { get; set; }
        public int ImplemantationPartnerId { get; set; }
        public string ImplemantationPartnerName { get; set; }
        public int? TeacherId { get; set; }
        public string TeacherName { get; set; }

        public FacilityType? FacilityType { get; set; }
        public FacilityStatus? FacilityStatus { get; set; }
        public TargetedPopulation TargetedPopulation { get; set; }

        public string Remarks { get; set; }


        public long EntityColumnId { get; set; }
        public string EntityColumnName { get; set; }
        public string EntityColumnNameInBangla { get; set; }
        public bool IsVersionColumn { get; set; }
        public bool IsFixed { get; set; }
        public bool? IsMultiValued { get; set; }
        public ColumnDataType DataType { get; set; }
        public long? ColumnListId { get; set; }
        public string ColumnListName { get; set; }
        public int ColumnOrder { get; set; }

        public long? ListItemId { get; set; }
        public string ListItemTitle { get; set; }
        public int? ListItemValue { get; set; }

        public long? PropertiesId { get; set; }
        public string PropertiesValue { get; set; }
        public CollectionStatus? PropertiesDataCollectionStatus { get; set; }

        public long  InstanceId { get; set; }
        public string InstanceName { get; set; }

        public CollectionStatus CollectionStatus { get; set; }
        public int? UpdatedBy { get; set; }

        public DateTime? ApproveDate { get; set; }
        public string ApproveByUserName { get; set; }
        public string ApproveByUserEmail { get; set; }
        public string ApproveByUserPhone { get; set; }
        public string TeacherEmail { get; set; }

    }
}
