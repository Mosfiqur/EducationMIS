using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IObjectiveIndicatorService
    {
        Task<ObjectiveIndicatorViewModel> Create(ObjectiveIndicatorCreateViewModel model);
        Task Update(ObjectiveIndicatorUpdateViewModel model);
    }

}
