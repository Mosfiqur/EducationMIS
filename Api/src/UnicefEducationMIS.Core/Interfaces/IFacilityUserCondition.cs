using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Interfaces
{
   public interface IFacilityUserCondition
    {
        IQueryable<FacilityRawViewModel> ApplyCondition(IQueryable<FacilityRawViewModel> source, long instanceId);

        IQueryable<Facility> ApplyCondition(IQueryable<Facility> source, long instanceId);
        IList<FacilityRawViewModel> ApplyCondition(IList<FacilityRawViewModel> source);

    }
}
