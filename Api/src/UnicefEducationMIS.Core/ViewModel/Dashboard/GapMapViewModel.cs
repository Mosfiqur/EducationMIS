using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using UnicefEducationMIS.Core.Models.Common;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel.Dashboard
{
    public class GapMapViewModel
    {
        public int CampId { get; set; }
        public string CampName { get; set; }
        public int StartYear { get; set; }
        public Month StartMonth { get; set; }
        public int EndYear { get; set; }
        public Month EndMonth { get; set; }

        public int PeopleInNeed { get; set; }
        public int Target { get; set; }
        public int Outreach { get; set; }
        public int Gap { get; set; }
        public List<ShapeCoordinate> ShapeCoordinates { get; set; }

        public GapMapViewModel()
        {
            ShapeCoordinates = new List<ShapeCoordinate>();            
        }

        public string FillColor { get; set; }
    }
}
