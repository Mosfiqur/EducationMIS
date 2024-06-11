using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Service.Import;

namespace UnicefEducationMIS.Service.Report
{
    public class BeneficiaryExporter : IBeneficiaryExporter
    {
        private readonly IEnvironment _env;
        #region Excel Column Names

        #endregion
        public BeneficiaryExporter(IEnvironment env)
        {
            _env = env;
        }

        public async Task<byte[]> ExportAll(List<BeneficiaryViewModel> beneficiaries)
        {
            byte[] fileBytes = null;
            var templateFilePath =
                Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName, FileNames.GlobalTemplateFile);
            using (var excel = ExcelAdapter.Open(templateFilePath))
            {
                excel.ReadStyles();
                var writer = new BeneficiarySheetWriter(excel);
                await writer.Write(beneficiaries, new List<IndicatorSelectViewModel>(), CreateColumns());
                fileBytes = excel.GetBytes();
            }
            return fileBytes;
        }

        public async Task<byte[]> ExportAll(List<BeneficiaryViewModel> beneficiaries, List<IndicatorSelectViewModel> indicators)
        {
            var columns = CreateColumns();

            byte[] fileBytes = null;
            var templateFilePath =
                Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName, FileNames.GlobalTemplateFile);
            using (var excel = ExcelAdapter.Open(templateFilePath))
            {
                excel.ReadStyles();
                var writer = new BeneficiarySheetWriter(excel);
                await writer.Write(beneficiaries, indicators.OrderBy(x=> x.ColumnOrder).ToList(), columns);
                fileBytes = excel.GetBytes();
            }
            return fileBytes;
        }

        public async Task<byte[]> ExportAggBeneficiaries(List<BeneficiaryViewModel> beneficiaries, List<IndicatorSelectViewModel> indicators)
        {
            var columns = CreateColumns();
            columns.Insert(0, new ReportCell("Instance Name", "InstanceName", ExcelStyleIndex.WrappedTextStyle));

            var orderedBeneficiaries = beneficiaries.OrderBy(x => x.InstanceId)
                .ThenBy(x => x.EntityId).ToList();

            var orderedIndicators = indicators
                .OrderBy(x => x.ColumnOrder).ToList();


            byte[] fileBytes = null;
            var templateFilePath =
                Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName, FileNames.GlobalTemplateFile);
            using (var excel = ExcelAdapter.Open(templateFilePath))
            {
                excel.ReadStyles();
                var writer = new BeneficiarySheetWriter(excel);

                await writer.Write(orderedBeneficiaries, orderedIndicators, columns);
                fileBytes = excel.GetBytes();
            }
            return fileBytes;
        }


        private List<ReportCell> CreateColumns() => new List<ReportCell>();
    }
}
