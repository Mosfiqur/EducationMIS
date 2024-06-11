using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class ScheduleViewModel
    {
        public int Id { get; set; }
        [Required]
        public string ScheduleName { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]        
        public ScheduleType ScheduleType { get; set; }
        [Required]
        public EntityType ScheduleFor { get; set; }
        public bool IsDeleted { get; set; }
        public ScheduleStatus Status { get; set; }
        [Required]
        public FrequencyViewModel Frequency { get; set; }
        public IEnumerable<InstanceViewModel> Instances { get; set; }
    }
}
