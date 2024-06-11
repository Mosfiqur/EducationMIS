using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Core.ViewModel
{
  public class AssignTeacherViewModel
    {
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = Messages.InvalidInstanceId)]
        public long InstanceId { get; set; }
        public List<long> FacilityIds { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int TeacherId { get; set; }
    }
}
