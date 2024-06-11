using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Reporting.Models;

namespace UnicefEducationMIS.Service.Report
{
    public interface IDamageReportExporter
    {
        Task<byte[]> ExportAll(
            DamageSummary damageSummary, 
            List<PartnerWiseDamage> partnerWiseDamages,
            List<YearlyDamageSummary> yearlyDamages
            );
    }


}