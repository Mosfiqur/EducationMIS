using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Service.Report
{
    public class SummeryReportConstants
    {

        public static readonly List<long> Indicators = new List<long>()
        {
            35,36,38,39,41,42,44,45,47,48,50,51,53,54,56,57,59,60,62,63
            ,65,66,68,69,71,72,74,75,77,78,80,81,83,84,86,87,89,90,92,93

            ,95,96,97,98

            ,22,23,24,26

            ,27,28,29,30,31,32,33,34

            ,99,100,101,102,103,104,105,106,107,108,109
        };

        public static Dictionary<AgeGroup, Dictionary<Gender, Dictionary<string, int>>> IndicatorAgeGroupWise = new Dictionary<AgeGroup, Dictionary<Gender, Dictionary<string, int>>>()
        {
            [AgeGroup.Three] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["host"] = 41,
                    ["refugee"] = 35
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["host"] = 44,
                    ["refugee"] = 38
                }
            },
            [AgeGroup.Four_Five] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["host"] = 53,
                    ["refugee"] = 47
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["host"] = 56,
                    ["refugee"] = 50
                }
            },
            [AgeGroup.Six_Fourteen] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["host"] = 65,
                    ["refugee"] = 59
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["host"] = 68,
                    ["refugee"] = 62
                }
            },
            [AgeGroup.Fifteen_Eighteen] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["host"] = 77,
                    ["refugee"] = 71
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["host"] = 80,
                    ["refugee"] = 74
                }
            },
            [AgeGroup.Nineteen_TwentyFour] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["host"] = 89,
                    ["refugee"] = 83
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["host"] = 92,
                    ["refugee"] = 86
                }
            }
        };
        public static Dictionary<AgeGroup, Dictionary<Gender, Dictionary<string, int>>> DisabilityIndicatorAgeGroupWise = new Dictionary<AgeGroup, Dictionary<Gender, Dictionary<string, int>>>()
        {
            [AgeGroup.Three] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["host"] = 42,
                    ["refugee"] = 36
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["host"] = 45,
                    ["refugee"] = 39
                }
            },
            [AgeGroup.Four_Five] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["host"] = 54,
                    ["refugee"] = 48
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["host"] = 57,
                    ["refugee"] = 51
                }
            },
            [AgeGroup.Six_Fourteen] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["host"] = 66,
                    ["refugee"] = 60
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["host"] = 69,
                    ["refugee"] = 63
                }
            },
            [AgeGroup.Fifteen_Eighteen] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["host"] = 78,
                    ["refugee"] = 72
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["host"] = 81,
                    ["refugee"] = 75
                }
            },
            [AgeGroup.Nineteen_TwentyFour] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["host"] = 90,
                    ["refugee"] = 84
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["host"] = 93,
                    ["refugee"] = 87
                }
            }
        };

        public static readonly List<long> DisabilityIndicatorList = new List<long>()
        {
            36,39,42,45,48,51,54,57,60,63,66,69,72,75,78,81,84,87,90,93
        };

        public const long ColRefugeeFemaleFacilitator = 95;
        public const long ColRefugeeMaleFacilitator = 96;
        public const long ColHostFemaleFacilitator = 97;
        public const long ColHostMaleFacilitator = 98;
        public static readonly List<long> FacilitatorIndicatorList = new List<long>()
        {
            ColRefugeeFemaleFacilitator,ColRefugeeMaleFacilitator,ColHostFemaleFacilitator,ColHostMaleFacilitator
        };

        public const long ColNumOfDrr = 99;
        public const long ColNumOfEduCommitee = 100;
        public const long ColFemaleRCareGiver = 101;
        public const long ColMaleRCareGiver = 102;
        public const long ColFemaleHCareGiver = 103;
        public const long ColMaleHCareGiver = 104;
        public const long ColRGirls3_24 = 105;
        public const long ColRBoys3_24 = 106;
        public const long ColBenefittingFromFood = 107;
        public const long ColBenefittingFromClassRoom = 108;
        public const long ColBenefittingFood = 109;

        public static readonly List<long> DrrCareGiverIndicators = new List<long>()
        {
            ColNumOfDrr  ,ColNumOfEduCommitee  ,ColFemaleRCareGiver  ,ColMaleRCareGiver  ,ColFemaleHCareGiver  ,ColMaleHCareGiver 
            ,ColRGirls3_24  ,ColRBoys3_24  ,ColBenefittingFromFood  ,ColBenefittingFromClassRoom ,ColBenefittingFood
        };

        public const long ColRGirlsLevel1 = 27;
        public const long ColRBoysLevel1 = 28;
        public const long ColRGirlsLevel2 = 29;
        public const long ColRBoysLevel2 = 30;
        public const long ColRGirlsLevel3 = 31;
        public const long ColRBoysLevel3 = 32;
        public const long ColRGirlsLevel4 = 33;
        public const long ColRBoysLevel4 = 34;
        public static readonly List<long> StudyLevelIndicators = new List<long>()
        {
            ColRGirlsLevel1,ColRBoysLevel1,ColRGirlsLevel2,ColRBoysLevel2
            ,ColRGirlsLevel3,ColRBoysLevel3,ColRGirlsLevel4,ColRBoysLevel4
        };

        public const long ColCommunityLatrines = 22;
        public const long ColBoysLatrines = 23;
        public const long ColGirlsLatrines = 24;
        public const long ColHandWashingStations = 26;
        public static readonly List<long> WashIndicator = new List<long>()
        {
            ColCommunityLatrines,ColBoysLatrines,ColGirlsLatrines,ColHandWashingStations
        };
    }
}
