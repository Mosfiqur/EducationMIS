using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Reporting.Models;

namespace UnicefEducationMIS.Service.Report
{
    public interface IDuplicationReportExporter
    {
        Task<byte[]> ExportAll(List<FailityWiseDuplicate> facilityWiseDupliates, List<StudentWiseDuplicate> studentWiseDuplicates);
    }

    
}