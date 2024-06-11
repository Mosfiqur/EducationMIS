using System.ComponentModel.DataAnnotations;

namespace UnicefEducationMIS.Core.ViewModel.Framework
{
    public class ObjectiveIndicatorUpdateViewModel : ObjectiveIndicatorCreateViewModel
    {
        [Required] 
        [Range(1, long.MaxValue)]
        public long Id { get; set; }
    }
}