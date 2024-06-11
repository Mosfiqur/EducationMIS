using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public class Instance: BaseModel<long>
    {
        public Instance()
        {
            Status = InstanceStatus.Pending;
            BeneciariesDataCollectionStatus = new HashSet<BeneficiaryDataCollectionStatus>();
            FacilitiesDataCollectionStatus = new HashSet<FacilityDataCollectionStatus>();
            FacilityDynamicCells = new HashSet<FacilityDynamicCell>();
        }
        public long ScheduleId { get; set; }
        public string Title { get; set; }
        public DateTime DataCollectionDate { get; set; }
        public InstanceStatus Status { get; set; }
        public Schedule Schedule { get; set; }

        public ICollection<BeneficiaryDataCollectionStatus> BeneciariesDataCollectionStatus { get; set; }
        public ICollection<FacilityDataCollectionStatus> FacilitiesDataCollectionStatus { get; set; }

        public ICollection<InstanceIndicator> InstanceIndicators { get; set; }
        public ICollection<FacilityDynamicCell> FacilityDynamicCells { get; set; }


        public DateTime GetStartDate()
        {
            return this.DataCollectionDate;
        }

        public DateTime GetEndDate()
        {
            var totalDayOfTheMonth = new DateTime(this.DataCollectionDate.Year, this.DataCollectionDate.Month, 1)
                .AddMonths(1).AddDays(-1).Day;
            return Schedule.ScheduleType == ScheduleType.Monthly ?
                new DateTime(this.DataCollectionDate.Year, this.DataCollectionDate.Month, totalDayOfTheMonth)
                : this.DataCollectionDate.AddDays(7 * Schedule.Frequency.Interval).AddDays(-1);
        }
    }
}
