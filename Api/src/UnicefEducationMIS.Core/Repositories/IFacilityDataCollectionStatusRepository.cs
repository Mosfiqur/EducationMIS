using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IFacilityDataCollectionStatusRepository : IBaseRepository<FacilityDataCollectionStatus, long>
    {
        Task ApproveFacility(FacilityApprovalViewModel model);
        Task<(List<FacilityDataCollectionStatus>,List<int>)> RecollectFacility(FacilityApprovalViewModel model);
    }
}
