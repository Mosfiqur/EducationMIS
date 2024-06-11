using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public partial class Beneficiary : BaseModel<long>
    {
        public Beneficiary()
        {
            BenificiaryDynamicCells = new HashSet<BeneficiaryDynamicCell>();
            BeneciaryDataCollectionStatuses = new HashSet<BeneficiaryDataCollectionStatus>();
        }

        public ICollection<BeneficiaryDynamicCell> BenificiaryDynamicCells { get; set; }
        public ICollection<BeneficiaryDataCollectionStatus> BeneciaryDataCollectionStatuses { get; set; }

        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public string UnhcrId { get; set; }
        [NotMapped]
        public Facility Facility { get; set; }

        [NotMapped]
        public string FatherName { get; set; }
        [NotMapped]
        public string MotherName { get; set; }
        [NotMapped]
        public string FCNId { get; set; }
        [NotMapped]
        public DateTime DateOfBirth { get; set; }
        [NotMapped]
        public Gender Sex { get; set; }
        [NotMapped]
        public bool Disabled { get; set; }
        [NotMapped]
        public LevelOfStudy LevelOfStudy { get; set; }
        [NotMapped]
        public DateTime EnrollmentDate { get; set; }
        [NotMapped]
        public int CampId { get; set; }
        [NotMapped]
        public string CampSsId { get; set; }
        [NotMapped]
        public int BlockId { get; set; }
        [NotMapped]
        public string BlockCode { get; set; }
        [NotMapped]
        public int SubBlockId { get; set; }
        [NotMapped]
        public string SubBlockName { get; set; }
        [NotMapped]
        public string Remarks { get; set; }

    }
}
