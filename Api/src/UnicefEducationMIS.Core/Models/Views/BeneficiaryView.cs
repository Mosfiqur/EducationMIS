using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models.Views
{
    public class BeneficiaryView
    {
        public long InstanceId { get; set; }
        public long Id { get; set; }

        public string UnhcrId { get; set; }
        public string Name { get; set; }

        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FCNId { get; set; }

        public DateTime DateOfBirth { get; set; }
        public Gender Sex { get; set; }
        public bool Disabled { get; set; }
        public LevelOfStudy LevelOfStudy { get; set; }

        public DateTime EnrollmentDate { get; set; }


        public long FacilityId { get; set; }
        //   public int FacilityCampId { get; set; }
        public int BeneficiaryCampId { get; set; }
        public int BlockId { get; set; }
        public int SubBlockId { get; set; }
        public string Remarks { get; set; }

        //public bool IsActive { get; set; }
        //public bool IsApproved { get; set; }

    }
}
