using System.Collections.Generic;

namespace UnicefEducationMIS.Service.Report
{
    public class CampWiseReportConstants
    {
        public const string ColCamnpName = "CampName";
        public const string ColPp = "List of PP";
        public const string ColIp = "List of IP";
        public const string ColTotalFacility = "total # of facility";
        public const string ColTotalPin = "Total PIN";
        public const string ColTotalTarget = "Total Target";
        public const string ColTotalOutreach = "Total outreach";
        public const string ColTotalFemaleRefugeeFacilitator = "Total Female Refugee Facilitator";
        public const string ColTotalMaleRefugeeFacilitator = "Total Male Refugee Facilitator";
        public const string ColTotalFemaleHostFacilitator = "Total Female Host Facilitator";
        public const string ColTotalMaleHostFacilitator = "Total Male Host Facilitator";

        public const string ColNoOfRohingyaGirlsLevel1 = "# of rohingya girls in level-1";
        public const string ColNoOfRohingyaBoysLevel1 = "# of rohingya boys in level-1";
        public const string ColNoOfRohingyaGirlsLevel2 = "# of rohingya girls in level-2";
        public const string ColNoOfRohingyaBoysLevel2 = "# of rohingya boys in level-2";
        public const string ColNoOfRohingyaGirlsLevel3 = "# of rohingya girls in level-3";
        public const string ColNoOfRohingyaBoysLevel3 = "# of rohingya boys in level-3";
        public const string ColNoOfRohingyaGirlsLevel4 = "# of rohingya girls in level-4";
        public const string ColNoOfRohingyaBoysLevel4 = "# of rohingya boys in level-4";

        public const string ColNoOfDrrAwarness = "Number of DRR awareness sessions conducted in HC (non-formal/community-based schools)";
        public const string ColNoOfLearningFacilityEstablished = "# of Rohingya Learning Facility Education Committees  established in rohingya camps";
        public const string ColNoOfRohingyaFemaleCaregivers = "# of female rohingya caregivers sensitized on child/youth rights, protection and parenting";
        public const string ColNoOfRohingyaMaleCaregivers = "# of male rohingya caregivers sensitized on child/youth rights, protection and parenting";
        public const string ColNoOfHcFemaleCaregivers = "# of female HC caregivers sensitized on child/youth rights, protection and parenting";
        public const string ColNoOfHcMaleCaregivers = "# of male HC caregivers sensitized on child/youth rights, protection and parenting";

        public const string ColRohingyaGirlsEngagedSocialCohesion = "# of rohingya girls aged 3-24 years old engaged in social cohesion initiatives";
        public const string ColRohingyaBoysEngagedSocialCohesion = "# of rohingya boys aged 3-24 years old engaged in social cohesion initiatives";

        public const string ColChildernBenefitingFromFood = "# of children benefitting from food";
        public const string ColNoOfChildernBenefitingFromSchool = "# of children benefitting from school/classroom/toilet rehabilitation";
        public const string ColChildernBenefitingFood = "# of children benefitting food";


        public static readonly List<string> FixedColumns = new List<string>
        {

ColCamnpName
,ColPp
,ColIp
,ColTotalFacility
,ColTotalPin
,ColTotalTarget
,ColTotalOutreach
,ColTotalFemaleRefugeeFacilitator
,ColTotalMaleRefugeeFacilitator
,ColTotalFemaleHostFacilitator
,ColTotalMaleHostFacilitator
,ColNoOfRohingyaGirlsLevel1
,ColNoOfRohingyaBoysLevel1
,ColNoOfRohingyaGirlsLevel2
,ColNoOfRohingyaBoysLevel2
,ColNoOfRohingyaGirlsLevel3
,ColNoOfRohingyaBoysLevel3
,ColNoOfRohingyaGirlsLevel4
,ColNoOfRohingyaBoysLevel4
,ColNoOfDrrAwarness
,ColNoOfLearningFacilityEstablished
,ColNoOfRohingyaFemaleCaregivers
,ColNoOfRohingyaMaleCaregivers
,ColNoOfHcFemaleCaregivers
,ColNoOfHcMaleCaregivers
,ColRohingyaGirlsEngagedSocialCohesion
,ColRohingyaBoysEngagedSocialCohesion
,ColChildernBenefitingFromFood
,ColNoOfChildernBenefitingFromSchool
,ColChildernBenefitingFood

        };

        public static readonly Dictionary<int, string> IndexToColumns = new Dictionary<int, string>
        {

  {0,ColCamnpName                          }
 ,{1,ColPp                                 },{2,ColIp                                 },{3,ColTotalFacility                      }
 ,{4,ColTotalPin                           },{5,ColTotalTarget                        },{6,ColTotalOutreach                      }
 ,{7,ColTotalFemaleRefugeeFacilitator      },{8,ColTotalMaleRefugeeFacilitator        },{9,ColTotalFemaleHostFacilitator         }
,{10,ColTotalMaleHostFacilitator           },{11,ColNoOfRohingyaGirlsLevel1            },{12,ColNoOfRohingyaBoysLevel1             }
,{13,ColNoOfRohingyaGirlsLevel2            },{14,ColNoOfRohingyaBoysLevel2             },{15,ColNoOfRohingyaGirlsLevel3            }
,{16,ColNoOfRohingyaBoysLevel3             },{17,ColNoOfRohingyaGirlsLevel4            },{18,ColNoOfRohingyaBoysLevel4             }
,{19,ColNoOfDrrAwarness                    },{20,ColNoOfLearningFacilityEstablished    },{21,ColNoOfRohingyaFemaleCaregivers       }
,{22,ColNoOfRohingyaMaleCaregivers         },{23,ColNoOfHcFemaleCaregivers             },{24,ColNoOfHcMaleCaregivers               },{25,ColRohingyaGirlsEngagedSocialCohesion }
,{26,ColRohingyaBoysEngagedSocialCohesion  },{27,ColChildernBenefitingFromFood         },{28,ColNoOfChildernBenefitingFromSchool   },{29,ColChildernBenefitingFood             }

        };

        public static readonly List<long> Indicators = new List<long>()
        { 95,96,97,98,27,28,29,30,31,32,33
            ,34,99,100,101,102,103,104,105,106,107,108,109
        };
        public const long TotalFemaleRefugeeFacilitator = 95;
        public const long TotalMaleRefugeeFacilitator = 96;
        public const long TotalFemaleHostFacilitator = 97;
        public const long TotalMaleHostFacilitator = 98;

        public const long NoOfRohingyaGirlsLevel1 = 27;
        public const long NoOfRohingyaBoysLevel1 = 28;
        public const long NoOfRohingyaGirlsLevel2 = 29;
        public const long NoOfRohingyaBoysLevel2 = 30;
        public const long NoOfRohingyaGirlsLevel3 = 31;
        public const long NoOfRohingyaBoysLevel3 = 32;
        public const long NoOfRohingyaGirlsLevel4 = 33;
        public const long NoOfRohingyaBoysLevel4 = 34;

        public const long NoOfDrrAwarness = 99;
        public const long NoOfLearningFacilityEstablished = 100;
        public const long NoOfRohingyaFemaleCaregivers = 101;
        public const long NoOfRohingyaMaleCaregivers = 102;
        public const long NoOfHcFemaleCaregivers = 103;
        public const long NoOfHcMaleCaregivers = 104;

        public const long RohingyaGirlsEngagedSocialCohesion = 105;
        public const long RohingyaBoysEngagedSocialCohesion = 106;

        public const long ChildernBenefitingFromFood = 107;
        public const long NoOfChildernBenefitingFromSchool = 108;
        public const long ChildernBenefitingFood = 109;
        public static readonly List<long> OutReachCalculationIndicator = new List<long>()
        { 27,28,29,30,31,32,33,34 };
    }
}
