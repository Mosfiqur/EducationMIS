using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Service.Report
{
    public interface IUserExporter
    {
        Task<byte[]> ExportUsers(List<UserViewModel> users, List<DynamicPropertiesViewModel> dynamicColumns);
    }
}