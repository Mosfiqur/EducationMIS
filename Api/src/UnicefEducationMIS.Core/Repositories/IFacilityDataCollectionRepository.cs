using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IFacilityDataCollectionRepository : IBaseRepository<FacilityDynamicCell, long>
    {
        IQueryable<FacilityRawViewModel> GetSubmittedData(SubmittedFacilityQueryModel model);
        Task<List<long>> GetPaginatedFacilityId(SubmittedFacilityQueryModel model);
        Task<int> GetTotal(SubmittedFacilityQueryModel model);
    }
}
