using System;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.QueryModel.Reporting;

namespace UnicefEducationMIS.Core.DomainServices
{
    public interface IReportService
    {
        Task<byte[]> Generate5wReport(string fileFullPath, long facilityInstanceId);
        Task<byte[]> GenerateCampWiseReport(string fileFullPath, long facilityInstanceId);
        Task<byte[]> GetDuplicationReport(DuplicationReportQueryModel model);
        Task<byte[]> GetGapAnalysisReport(GapAnalysisReportQueryModel model);
        Task<byte[]> GetDamageReport(DamageReportQueryModel model);
        Task<byte[]> GetSummaryReport(SummaryReportQueryModel model);
    }
}
