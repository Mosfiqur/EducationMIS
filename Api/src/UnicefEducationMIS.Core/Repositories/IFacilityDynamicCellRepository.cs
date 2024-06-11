using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ValueObjects;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IFacilityDynamicCellRepository : IBaseRepository<FacilityDynamicCell, long>
    {
        Task Save(FacilityDynamicCellAddViewModel dynamicColumn);
        Task Delete(FacilityDynamicCellDeleteViewModel dynamicCell);                
        Task ImportVersionData(List<FacilityDynamicCellAddViewModel> dynamicCells, CollectionStatus collectionStatus, long instanceId);
        
    }
}
