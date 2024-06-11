using System.Collections.Generic;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.AppConstants
{
    public class AutoCalculatedIndicatorConstants
    {
        public const int No_of_rohingya_girls_in_level_1 = 27;
        public const int No_of_rohingya_boys_in_level_1 = 28;
        public const int No_of_rohingya_girls_in_level_2 = 29;
        public const int No_of_rohingya_boys_in_level_2 = 30;
        public const int No_of_rohingya_girls_in_level_3 = 31;
        public const int No_of_rohingya_boys_in_level_3 = 32;
        public const int No_of_rohingya_girls_in_level_4 = 33;
        public const int No_of_rohingya_boys_in_level_4 = 34;
        public const int No_of_rohingya_girls_3_enrolled_in_LF = 35;
        public const int No_of_rohingya_girls_3_with_disability_enrolled_in_LF = 36;
        public const int No_of_rohingya_girls_3_receiving_edu_materials = 37;
        public const int No_of_rohingya_boys_3_enrolled_in_LF = 38;
        public const int No_of_rohingya_boys_3_with_disability_enrolled = 39;
        public const int No_of_rohingya_boys_3_receiving_edu_materials = 40;
        public const int No_of_rohingya_girls_4_5_enrolled_in_LF = 47;
        public const int No_of_rohingya_girls_4_5_with_disability_enrolled_in_LF = 48;
        public const int No_of_rohingya_girls_4_5_receiving_edu_materials = 49;
        public const int No_of_rohingya_boys_4_5_enrolled_in_LF = 50;
        public const int No_of_rohingya_boys_4_5_with_disability_enrolled = 51;
        public const int No_of_rohingya_boys_4_5_receiving_edu_materials = 52;
        public const int No_of_rohingya_girls_6_14_enrolled_in_LF = 59;
        public const int No_of_rohingya_girls_6_14_with_disability_enrolled_in_LF = 60;
        public const int No_of_rohingya_girls_6_14_receiving_edu_materials = 61;
        public const int No_of_rohingya_boys_6_14_enrolled_in_LF = 62;
        public const int No_of_rohingya_boys_6_14_with_disability_enrolled = 63;
        public const int No_of_rohingya_boys_6_14_receiving_edu_materials = 64;
        public const int No_of_rohingya_girls_15_18_enrolled_in_LF = 71;
        public const int No_of_rohingya_girls_15_18_with_disability_enrolled_in_LF = 72;
        public const int No_of_rohingya_girls_15_18_receiving_edu_materials = 73;
        public const int No_of_rohingya_boys_15_18_enrolled_in_LF = 74;
        public const int No_of_rohingya_boys_15_18_with_disability_enrolled = 75;
        public const int No_of_rohingya_boys_15_18_receiving_edu_materials = 76;
        public const int No_of_rohingya_girls_19_24_enrolled_in_LF = 83;
        public const int No_of_rohingya_girls_19_24_with_disability_enrolled_in_LF = 84;
        public const int No_of_rohingya_girls_19_24_receiving_edu_materials = 85;
        public const int No_of_rohingya_boys_19_24_enrolled_in_LF = 86;
        public const int No_of_rohingya_boys_19_24_with_disability_enrolled = 87;
        public const int No_of_rohingya_boys_19_24_receiving_edu_materials = 88;

        //public static Dictionary<ValueObjects.AgeGroup, List<int>> AgeIndicators = new Dictionary<ValueObjects.AgeGroup, List<int>>()
        //{
        //    [ValueObjects.AgeGroup.Three] = new List<int> { 17, 18, 19, 20, 21, 22 },
        //    [ValueObjects.AgeGroup.Four_Five] = new List<int> { 29, 30, 31, 32, 33, 34 }
        //};

        public static Dictionary<LevelOfStudy, Dictionary<Gender, int>> IndicatorLevelOfStudyWise = new Dictionary<LevelOfStudy, Dictionary<Gender, int>>()
        {
            [LevelOfStudy.Level_1] = new Dictionary<Gender, int>
            {
                [Gender.Female] = No_of_rohingya_girls_in_level_1,
                [Gender.Male] = No_of_rohingya_boys_in_level_1
            },
            [LevelOfStudy.Level_2] = new Dictionary<Gender, int>
            {
                [Gender.Female] = No_of_rohingya_girls_in_level_2,
                [Gender.Male] = No_of_rohingya_boys_in_level_2
            },
            [LevelOfStudy.Level_3] = new Dictionary<Gender, int>
            {
                [Gender.Female] = No_of_rohingya_girls_in_level_3,
                [Gender.Male] = No_of_rohingya_boys_in_level_3
            },
            [LevelOfStudy.Level_4] = new Dictionary<Gender, int>
            {
                [Gender.Female] = No_of_rohingya_girls_in_level_4,
                [Gender.Male] = No_of_rohingya_boys_in_level_4
            }
        };
        public static Dictionary<ValueObjects.AgeGroup, Dictionary<Gender, Dictionary<string, int>>> IndicatorAgeGroupWise = new Dictionary<AgeGroup, Dictionary<Gender, Dictionary<string, int>>>()
        {
            [AgeGroup.Three] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["Enroll"] = No_of_rohingya_girls_3_enrolled_in_LF,
                    ["DisableEnroll"] = No_of_rohingya_girls_3_with_disability_enrolled_in_LF,
                    ["EduMaterialReceived"] = No_of_rohingya_girls_3_receiving_edu_materials
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["Enroll"] = No_of_rohingya_boys_3_enrolled_in_LF,
                    ["DisableEnroll"] = No_of_rohingya_boys_3_with_disability_enrolled,
                    ["EduMaterialReceived"] = No_of_rohingya_boys_3_receiving_edu_materials
                }
            },
            [AgeGroup.Four_Five] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["Enroll"] = No_of_rohingya_girls_4_5_enrolled_in_LF,
                    ["DisableEnroll"] = No_of_rohingya_girls_4_5_with_disability_enrolled_in_LF,
                    ["EduMaterialReceived"] = No_of_rohingya_girls_4_5_receiving_edu_materials
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["Enroll"] = No_of_rohingya_boys_4_5_enrolled_in_LF,
                    ["DisableEnroll"] = No_of_rohingya_boys_4_5_with_disability_enrolled,
                    ["EduMaterialReceived"] = No_of_rohingya_boys_4_5_receiving_edu_materials
                }
            },
            [AgeGroup.Six_Fourteen] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["Enroll"] = No_of_rohingya_girls_6_14_enrolled_in_LF,
                    ["DisableEnroll"] = No_of_rohingya_girls_6_14_with_disability_enrolled_in_LF,
                    ["EduMaterialReceived"] = No_of_rohingya_girls_6_14_receiving_edu_materials
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["Enroll"] = No_of_rohingya_boys_6_14_enrolled_in_LF,
                    ["DisableEnroll"] = No_of_rohingya_boys_6_14_with_disability_enrolled,
                    ["EduMaterialReceived"] = No_of_rohingya_boys_6_14_receiving_edu_materials
                }
            },
            [AgeGroup.Fifteen_Eighteen] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["Enroll"] = No_of_rohingya_girls_15_18_enrolled_in_LF,
                    ["DisableEnroll"] = No_of_rohingya_girls_15_18_with_disability_enrolled_in_LF,
                    ["EduMaterialReceived"] = No_of_rohingya_girls_15_18_receiving_edu_materials
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["Enroll"] = No_of_rohingya_boys_15_18_enrolled_in_LF,
                    ["DisableEnroll"] = No_of_rohingya_boys_15_18_with_disability_enrolled,
                    ["EduMaterialReceived"] = No_of_rohingya_boys_15_18_receiving_edu_materials
                }
            },
            [AgeGroup.Nineteen_TwentyFour] = new Dictionary<Gender, Dictionary<string, int>>()
            {
                [Gender.Female] = new Dictionary<string, int>()
                {
                    ["Enroll"] = No_of_rohingya_girls_19_24_enrolled_in_LF,
                    ["DisableEnroll"] = No_of_rohingya_girls_19_24_with_disability_enrolled_in_LF,
                    ["EduMaterialReceived"] = No_of_rohingya_girls_19_24_receiving_edu_materials
                },
                [Gender.Male] = new Dictionary<string, int>()
                {
                    ["Enroll"] = No_of_rohingya_boys_19_24_enrolled_in_LF,
                    ["DisableEnroll"] = No_of_rohingya_boys_19_24_with_disability_enrolled,
                    ["EduMaterialReceived"] = No_of_rohingya_boys_19_24_receiving_edu_materials
                }
            }
        };


        //    new 
        //{
        //    [ValueObjects.AgeGroup.Three] = new Dictionary<string, int>()
        //    {
        //        ["Enroll"]= 17,
        //        ["DisableEnroll"]=
        //    }
        //    [ValueObjects.AgeGroup.Four_Five] = new List<int> { 29, 30, 31, 32, 33, 34 }
        //};
    }
}
