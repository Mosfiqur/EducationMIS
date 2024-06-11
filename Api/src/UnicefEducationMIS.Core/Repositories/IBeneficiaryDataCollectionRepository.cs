using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IBeneficiaryDataCollectionRepository : IBaseRepository<BeneficiaryDynamicCell, long>
    {
        IQueryable<BeneficiaryRawViewModel> GetDeactivateBeneficiaryData(SubmittedBeneficiaryQueryModel model);
        IQueryable<BeneficiaryRawViewModel> GetSubmittedData(SubmittedBeneficiaryQueryModel model);
        Task<List<long>> GetPaginatedBeneficiaryId(SubmittedBeneficiaryQueryModel model);
        Task<List<long>> GetSubmittedBeneficiaryId(SubmittedBeneficiaryQueryModel model, int skip, int take);
        Task<int> GetTotal(SubmittedBeneficiaryQueryModel model);

    }
}
