using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class GapMapQueryModel
    {
        public List<int> Years { get; set; }
        public List<int> AgeGroupIds { get; set; }
        public List<Gender> Genders { get; set; }
        public List<int> CampIds { get; set; }

        public GapMapQueryModel()
        {
            Years = new List<int>();
            AgeGroupIds = new List<int>();
            Genders = new List<Gender>();
            CampIds = new List<int>();
        }

    }
}
