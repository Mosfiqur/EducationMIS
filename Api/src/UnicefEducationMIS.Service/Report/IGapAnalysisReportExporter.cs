using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Reporting.Models;

namespace UnicefEducationMIS.Service.Report
{
    public interface IGapAnalysisReportExporter
    {
        Task<byte[]> ExportAll(List<GapAnalysisReport> data);
    }
}