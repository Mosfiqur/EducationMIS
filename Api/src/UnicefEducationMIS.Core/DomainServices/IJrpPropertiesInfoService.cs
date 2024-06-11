using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.QueryModel;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IJrpPropertiesInfoService
    {
        Task Add(JrpParameterInfoViewModel jrpParameterInfo);
        Task Update(JrpParameterInfoViewModel jrpParameterInfo);
        Task Delete(int entityId);

        Task<List<JrpParameterInfoViewModel>> Get(BaseQueryModel baseQueryModel);

        Task<JrpChartViewModel> GetJrpChartData(JrpPropertiesWithFilterViewModel jrpReportFilter);
    }
}
