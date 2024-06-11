using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IFacilityDynamicCellService
    {
        Task Save(FacilityDynamicCellAddViewModel model);
        Task Delete(FacilityDynamicCellDeleteViewModel model);        
        Task ImportVersionData(List<FacilityDynamicCellAddViewModel> dynamicCells, CollectionStatus collectionStatus, long instanceId);
    }
}
