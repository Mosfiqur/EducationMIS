using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.Repositories
{
    public interface IJrpPropertiesInfoRepository: IBaseRepository<JrpParameterInfo, int>
    {
        JrpChartData GetJrpChartData(JrpPropertiesWithFilterViewModel jrpReportFilter, long benficiaryInstanceId, long facilityInstanceId); 
    }
}
