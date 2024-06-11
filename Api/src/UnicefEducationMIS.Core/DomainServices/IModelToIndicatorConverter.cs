using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IModelToIndicatorConverter
    {
        IEnumerable<DynamicCellViewModel> FacilityToDynamicCell(CreateFacilityViewModel model);
        IEnumerable<DynamicCellViewModel> AssignTeacherToDynamicCell(AssignTeacherViewModel model);
        void ReplaceFacilityFixedIndicatorIdsWithValues(FacilityViewModel model);

        CreateFacilityViewModel DynamicCellToCreateFacilityViewModels(
            FacilityDynamicCellAddViewModel model);

        void ReplaceBeneficiaryFixedIndicatorIdsWithValues(List<BeneficiaryViewModel> beneficiaries);
    }
}
