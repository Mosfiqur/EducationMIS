using System.Collections.Generic;

namespace UnicefEducationMIS.Service.Import
{
    public class BeneficiaryVersionDataColumnNames
    {
        public const string Id = "Id";
        public const string UnhcrId = "proGres ID";
        public const string BeneficiaryName = "Beneficiary Name";
        public const string FacilityId = "Facility Id";
        public const string FacilityName = "Facility Name";

        public static readonly List<string> AllColumns = new List<string>
        {
            Id, BeneficiaryName , UnhcrId , FacilityId, FacilityName  
        };

    }
}
