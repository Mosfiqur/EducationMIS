using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace UnicefEducationMIS.Core.ValueObjects
{
    public enum FacilityType
    {
        [Description("Learning Center(LC)")]
        LearningCenter = 1,
        [Description("Community Based Learning Facility")]
        Community_Based_Learning_Facility = 2,
        [Description("Cross Sectoral Shared Learning Facility (CSSLF)")]
        Cross_Sectoral_Shared_Learning_Facility = 3

    }
}
