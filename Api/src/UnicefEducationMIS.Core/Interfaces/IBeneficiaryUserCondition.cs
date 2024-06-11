using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.Models.Views;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Interfaces
{
    public interface IBeneficiaryUserCondition
    {
        IQueryable<BeneficiaryRawViewModel> ApplyCondition(IQueryable<BeneficiaryRawViewModel> source, long instanceId);
        IQueryable<BeneficairyObjectViewModel> ApplyCondition(IQueryable<BeneficairyObjectViewModel> source, long instanceId);
        IQueryable<BeneficiaryView> ApplyCondition(IQueryable<BeneficiaryView> source, long instanceId);

        Task<List<BeneficiaryRawViewModel>> ApplyCondition(List<BeneficiaryRawViewModel> source, long instanceId);
    }
}
