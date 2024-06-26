﻿using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class BeneficiaryEditViewModel
    {
        public long Id { get; set; }
        public string UnhcrId { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string FCNId { get; set; }
        public string DateOfBirth { get; set; }
        public Gender Sex { get; set; }
        public bool Disabled { get; set; }
        public LevelOfStudy LevelOfStudy { get; set; }
        public string EnrollmentDate { get; set; }




        public long FacilityId { get; set; }
        public string FacilityCode { get; set; }
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
    }
}
