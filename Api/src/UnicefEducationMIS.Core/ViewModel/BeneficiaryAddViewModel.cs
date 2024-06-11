using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class BeneficiaryAddViewModel
    {
        public long Id { get; set; }
        [Required]
        public string UnhcrId { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        [Required]
        public string FCNId { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public Gender Sex { get; set; }
        [Required]
        public bool Disabled { get; set; }
        [Required]
        public string LevelOfStudy { get; set; }
        [Required]
        public DateTime EnrollmentDate { get; set; }

        [Required]
        public long FacilityId { get; set; }
        //public int? FacilityCampId { get; set; }
        [Required]
        public int BeneficiaryCampId { get; set; }
        [Required]
        public int BlockId { get; set; }
        [Required]
        public int SubBlockId { get; set; }

        public string Remarks { get; set; }

       // public bool IsApproved { get; set; }

        [Required]
        public long InstanceId { get; set; }

        //public List<BeneficiaryDynamicCellViewModel> DynamicCell { get; set; }


    }
}
