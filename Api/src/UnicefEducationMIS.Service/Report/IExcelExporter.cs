using System.Collections.Generic;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.Import;

namespace UnicefEducationMIS.Service.Report
{
    public interface IExcelExporter<T>
    {
        Task<byte[]> ExportAll(List<T> entities, List<ExcelCell> columns);
    }
}
