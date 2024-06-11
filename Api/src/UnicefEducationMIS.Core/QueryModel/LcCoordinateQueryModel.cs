using System.Collections.Generic;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class LcCoordinateQueryModel
    {
        public List<int> SelectedCamps { get; set; }
        public List<int> SelectedIPs { get; set; }
        public List<int> SelectedPPs { get; set; }
        public List<int> SelectedUnions { get; set; }
        public List<int> SelectedUpazilas { get; set; }
        public List<TargetedPopulation> SelectedTPs { get; set; }    
        
        public LcCoordinateQueryModel()
        {
            SelectedCamps = new List<int>();
            SelectedIPs = new List<int>();
            SelectedPPs = new List<int>();
            SelectedUnions = new List<int>();
            SelectedUpazilas = new List<int>();
            SelectedTPs = new List<TargetedPopulation>();            
        }
    }
}
