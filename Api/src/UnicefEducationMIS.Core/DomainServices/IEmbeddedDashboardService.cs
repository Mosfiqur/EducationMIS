using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IEmbeddedDashboardService
    {
        Task Save(EmbeddedDashboardViewModel embaddedDashboard);
        Task Update(EmbeddedDashboardViewModel embaddedDashboard);
        Task Delete(int id);
        Task<List<EmbeddedDashboardViewModel>> GetAll();
    }
}
