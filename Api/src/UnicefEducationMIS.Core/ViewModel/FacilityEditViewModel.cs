using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class FacilityEditViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FacilityCode { get; set; }
        public TargetedPopulation TargetedPopulation { get; set; }
        public FacilityStatus? FacilityStatus { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Donors { get; set; }
        public string NonEducationPartner { get; set; }


        public int ProgramPartnerId { get; set; }
        public string ProgramPartnerName { get; set; }
        public int ImplementationPartnerId { get; set; }
        public string ImplementationPartnerName { get; set; }
        public int UnionId { get; set; }
        public string UnionName { get; set; }
        public int UpazilaId { get; set; }
        public string UpazilaName { get; set; }
        public int? CampId { get; set; }
        public string CampName { get; set; }
        public string CampSSID { get; set; }
        public string ParaName { get; set; }
        public string ParaId { get; set; }
        public int BlockId { get; set; }
        public string BlockName { get; set; }

        public FacilityType? FacilityType { get; set; }

        public int? TeacherId { get; set; }
        public string TeacherName { get; set; }
        public string TeacherEmail { get; set; }
        public string TeacherPhone { get; set; }
        
        public int? NoOfLfEstablishedInRohingyaCommunity { get; set; }
        public int? NoOfClassroomsInLfInRohingyaCommunity { get; set; }
        public int? NoOfShiftsInLfInRohingyaCommunity { get; set; }
        public int? NoOfFunctionalCommunityLatrinesNearLF { get; set; }
        public int? NoOfLatrinesEstablishedForBoys { get; set; }
        public int? NoOfLatrinesEstablishedEorGirls { get; set; }
        public string IDofTheLatrinesAccessibleToTheLC { get; set; }
        public int? NoOfHandwashingStationEstablished { get; set; }
        public int? NoOfFemaleRohingyaLearningFacilitatorsTrained { get; set; }
        public int? NoOfMaleRohingyaLearningFacilitatorsTrained { get; set; }
        public int? NoOfFemaleHcLearningFacilitatorsTrained { get; set; }
        public int? NoOfMaleHcLearningFacilitatorsTrained { get; set; }
        public int? NoOfDrrAwarenessSessionsConductedInHc { get; set; }
        public int? NoOfRohingyaLearningFacilityEducationCommitteesEstablishedInRohingyaCamps { get; set; }
        public int? NoOfFemaleRohingyaCaregiversSensitizedOnCYRPAP { get; set; }
        public int? NoOfMaleRohingyaCaregiversSensitizedOnCYRPAP { get; set; }
        public int? NoOfFemaleHcCaregiversSensitizedOnCYRPAP { get; set; }
        public int? NoOfMaleHcCaregiversSensitizedOnCYRPAP { get; set; }
        public int? NoOfRohingyaGirlsAged3_24YearsOldEngagedInSCI { get; set; }
        public int? NoOfRohingyaBoysAged3_24YearsOldEngagedInSCI { get; set; }
        public int? NoOfChildrenBenefittingFromFood { get; set; }
        public int? NoOfChildrenBenefittingFromSchool_Classroom_ToiletRehabilitation { get; set; }
        public string OtherRapidIntervention { get; set; }
        public string Remarks { get; set; }

    }
}
