using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ResponseModel;
using UnicefEducationMIS.Core.ViewModel.Framework;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IBudgetFrameworkService
    {
        Task Add(BudgetFrameworkCreateViewModel model);
        Task Update(BudgetFrameworkUpdateViewModel model);
        Task Delete(long budgetId);
        Task DeleteMultiple(List<long> budgetIds);
        Task<PagedResponse<BudgetFrameworkViewModel>> GetAllBudgets(BaseQueryModel model);

        Task InsertDynamicCell(BudgetDynamicCellInsertViewModel model);
        Task DeleteDynamicCell(BudgetDynamicCellViewModel id);

        Task<ProjectViewModel> AddProject(ProjectViewModel model);
        Task<DonorViewModel> AddDonor(DonorViewModel model);

        Task<PagedResponse<ProjectViewModel>> GetAllProjects(BaseQueryModel model);
        Task<PagedResponse<DonorViewModel>> GetAllDonors(BaseQueryModel model);
        Task<BudgetFrameworkViewModel> GetById(long id);
    }
}
