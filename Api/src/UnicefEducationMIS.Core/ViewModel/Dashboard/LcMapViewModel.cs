using System.Collections.Generic;
using System.Linq;
using UnicefEducationMIS.Core.Models.Common;

namespace UnicefEducationMIS.Core.ViewModel.Dashboard
{
    public class LcMapViewModel
    {

        public int CampId { get; set; }
        public string CampName { get; set; }
        public int MinRadius { get; set; } = 20;
        public int MaxRadius { get; set; } = 100;

        public List<ShapeCoordinate> CampCoordinates { get; set; }
        public List<LearningCenterViewModel> LearningCenters { get; set; }

        public LcMapViewModel()
        {
            CampCoordinates = new List<ShapeCoordinate>();
            LearningCenters = new List<LearningCenterViewModel>();
        }

        public void CalculateRadius ()
        {

            if(LearningCenters.Count == 0)
            {
                return;
            }
            var maxBenef = this.LearningCenters.Select(x => x.NumberOfBeneficiaries).Max();
            var minBenef = this.LearningCenters.Select(x => x.NumberOfBeneficiaries).Min();

            int step = (maxBenef - minBenef) / (MaxRadius - MinRadius);

            step = step > 0 ? step : 1;

            this.LearningCenters.ForEach(x =>
            {
                x.Radius = MinRadius + (x.NumberOfBeneficiaries - minBenef) / step;
            });
        }

        
    }
}
