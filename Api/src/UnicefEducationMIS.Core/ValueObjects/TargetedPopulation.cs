using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace UnicefEducationMIS.Core.ValueObjects
{
    public enum TargetedPopulation
    {
        [Description("Host Communities")]
        Host_Communities = 1,
        [Description("Refugee Communities")]
        Refugee_Communities = 2,
        [Description("Both Communities")]
        Both_Communities = 3
    }
}
