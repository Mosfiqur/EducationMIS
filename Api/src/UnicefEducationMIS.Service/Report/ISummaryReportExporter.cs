using System.Threading.Tasks;

namespace UnicefEducationMIS.Service.Report
{
    public interface ISummaryReportExporter
    {
        Task<byte[]> ExportAll(Core.Reporting.Models.SummaryReport summaryReport);
    }


}