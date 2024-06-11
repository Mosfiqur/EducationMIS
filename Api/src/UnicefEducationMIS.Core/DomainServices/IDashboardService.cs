using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ViewModel.Dashboard;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IDashboardService
    {
        Task<TotalCountsViewModel> GetTotalCounts();
        Task<List<GapMapViewModel>> GetGapMaps(GapMapQueryModel query);
        Task<List<LcMapViewModel>> GetLcCoordinates(LcCoordinateQueryModel query);

    }
}
