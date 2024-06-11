using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
   public class BeneficiaryPreCollectionViewModel
    {
        public long BeneficiaryId { get; set; }
        public string BEneficairyName { get; set; }
        public long FacilityId { get; set; }
        public string FacilityName { get; set; }
        public CollectionStatus CollectionStatus { get; set; }
    }
}
