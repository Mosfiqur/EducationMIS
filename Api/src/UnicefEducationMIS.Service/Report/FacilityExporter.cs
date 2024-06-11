using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.DomainServices;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.Models;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Service.Import;

namespace UnicefEducationMIS.Service.Report
{
    public class FacilityExporter : IFacilityExporter
    {
        private readonly IEnvironment _env;
        private readonly IModelToIndicatorConverter _modelToIndicator;
        public FacilityExporter(IEnvironment env, IModelToIndicatorConverter modelToIndicator)
        {
            _env = env;
            _modelToIndicator = modelToIndicator;
        }

        public async Task<byte[]> ExportFacilities(List<FacilityViewModel> facilities)
        {
            byte[] fileBytes = null;
            var templateFilePath =
                Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName, FileNames.GlobalTemplateFile);
            using (var excel = ExcelAdapter.Open(templateFilePath))
            {
                excel.ReadStyles();
                var writer = new FacilitySheetWriter(excel);
                await writer.Write(facilities, new List<IndicatorSelectViewModel>(), FacilityVersionedDataColumns());
                fileBytes = excel.GetBytes();
            }
            return fileBytes;
        }

        public async Task<byte[]> ExportVersionedFacilities(List<FacilityViewModel> facilities, List<IndicatorSelectViewModel> indicators)
        {
            byte[] fileBytes = null;
            var templateFilePath =
                Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName, FileNames.GlobalTemplateFile);
            using (var excel = ExcelAdapter.Open(templateFilePath))
            {
                excel.ReadStyles();
                var writer = new FacilitySheetWriter(excel);

                facilities.ForEach(_modelToIndicator.ReplaceFacilityFixedIndicatorIdsWithValues);
                await writer.Write(facilities, indicators.OrderBy(x=> x.ColumnOrder).ToList(), FacilityVersionedDataColumns());
                fileBytes = excel.GetBytes();
            }
            return fileBytes;
        }

        public async Task<byte[]> ExportAggregatedFacilities(List<FacilityViewModel> facilities, List<IndicatorSelectViewModel> indicators)
        {

            var columns = FacilityVersionedDataColumns();
            columns.Insert(0, new ReportCell("Instance Name", "InstanceName", ExcelStyleIndex.WrappedTextStyle));
            
            byte[] fileBytes = null;
            var templateFilePath =
                Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName, FileNames.GlobalTemplateFile);
            using (var excel = ExcelAdapter.Open(templateFilePath))
            {
                excel.ReadStyles();
                var writer = new FacilitySheetWriter(excel);
                facilities.ForEach(_modelToIndicator.ReplaceFacilityFixedIndicatorIdsWithValues);
                await writer.Write(facilities, indicators.OrderBy(x=> x.IndicatorName).ToList(), columns);
                fileBytes = excel.GetBytes();
            }
            return fileBytes;
        }

        private List<ReportCell> FacilityVersionedDataColumns()
        {

            return new List<ReportCell>();
        }

    }
}