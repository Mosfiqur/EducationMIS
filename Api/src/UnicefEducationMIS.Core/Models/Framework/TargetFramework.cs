using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models.Framework
{
    public class TargetFramework : BaseModel<long>
    {
        public int? CampId { get; set; }

        public Gender Gender { get; set; }
        public int AgeGroupId { get; set; }
        public AgeGroup AgeGroup { get; set; }
        public int PeopleInNeed { get; set; }
        public int Target { get; set; }
        public int StartYear { get; set; }
        public Month StartMonth { get; set; }
        public int EndYear { get; set; }
        public Month EndMonth { get; set; }

        public int UpazilaId { get; set; }
        public int UnionId { get; set; }
        public TargetedPopulation TargetedPopulation { get; set; }


        public TargetFramework()
        {
            DynamicCells = new HashSet<TargetFrameworkDynamicCell>();
        }
        public Camp Camp { get; set; }
        public ICollection<TargetFrameworkDynamicCell> DynamicCells { get; set; }
    }
}
