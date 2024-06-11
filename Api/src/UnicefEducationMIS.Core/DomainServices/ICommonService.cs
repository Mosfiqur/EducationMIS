using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.QueryModel.Common;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel.Common;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface ICommonService
    {
        Task<PagedResponse<CampViewModel>> GetCamps(CampQueryModel model);
        Task<PagedResponse<BlockViewModel>> GetBlocks(BlockQueryModel model);
        Task<PagedResponse<SubBlockViewModel>> GetSubBlocks(SubBlockQueryModel model);
        Task<PagedResponse<DistrictViewModel>> GetDistricts(BaseQueryModel model);
        Task<PagedResponse<UnionViewModel>> GetUnions(UnionQueryModel model);
        Task<PagedResponse<UpazilaViewModel>> GetUpazilas(UpazilaQueryModel model);
        Task<PagedResponse<AgeGroupViewModel>> GetAgeGroups(BaseQueryModel model);
        Task<PagedResponse<ReportingFrequencyViewModel>> GetReportingFrequencies(BaseQueryModel model);
        Task<PagedResponse<CampBlockSubBlockViewModel>> GetCampsWithBlockSubBlock(BaseQueryModel model);
    }
}
