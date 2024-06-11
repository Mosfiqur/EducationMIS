using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class BeneficiaryRawViewModel
    {
       // public string BeneficiaryName { get; set; }

        public long EntityId { get; set; }
        public string UnhcrId { get; set; }
        public long BeneficiaryId { get; set; }
        public string BeneficiaryName { get; set; }
        public string FCNId { get; set; }

        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Sex { get; set; }
        public bool Disabled { get; set; }
        public LevelOfStudy LevelOfStudy { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public long FacilityId { get; set; }
        public string FacilityName { get; set; }
        public int? FacilityCampId { get; set; }
        public string FacilityCampName { get; set; }
        public int BeneficiaryCampId { get; set; }
        public string BeneficiaryCampName { get; set; }

        public int BlockId { get; set; }
        public string BlockName { get; set; }
        public int SubBlockId { get; set; }
        public string SubBlockName { get; set; }

        public int ProgrammingPartnerId { get; set; }
        public string ProgrammingPartnerName { get; set; }
        public int ImplemantationPartnerId { get; set; }
        public string ImplemantationPartnerName { get; set; }

        public string Remarks { get; set; }

        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }

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
        public long InstanceId { get; set; }
        public string InstanceName { get; set; }
        public string BlockCode { get; set; }
        public string FacilityCode { get; set; }
        public string CampSsId { get; set; }
        public CollectionStatus CollectionStatus { get; set; }
    }
}
