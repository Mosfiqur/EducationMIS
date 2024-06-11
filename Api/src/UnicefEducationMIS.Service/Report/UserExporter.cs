using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnicefEducationMIS.Core.AppConstants;
using UnicefEducationMIS.Core.Import;
using UnicefEducationMIS.Core.Interfaces;
using UnicefEducationMIS.Core.ViewModel;
using UnicefEducationMIS.Service.Import;

namespace UnicefEducationMIS.Service.Report
{
    public class UserExporter : IUserExporter
    {
        private readonly IEnvironment _env;

        public UserExporter(IEnvironment env)
        {
            _env = env;
        }

        public async Task<byte[]> ExportUsers(List<UserViewModel> users, List<DynamicPropertiesViewModel> dynamicColumns)
        {
            byte[] fileBytes = null;
            var templateFilePath =
                Path.Combine(_env.GetRootPath(), FileNames.ImportFolderName, FileNames.GlobalTemplateFile);
            using (var excel = ExcelAdapter.Open(templateFilePath))
            {
                excel.ReadStyles();

                var writer = new UserSheetWriter(excel);
                await writer.Write(users, GetFixedColumns(), dynamicColumns);
                fileBytes = excel.GetBytes();
            }
            return fileBytes;
        }

        private List<ReportCell> GetFixedColumns()
        {
            return new List<ReportCell>()
            {
                new ReportCell(UserExcelColumns.FullName, "FullName", ExcelStyleIndex.WrappedTextStyle),
                new ReportCell(UserExcelColumns.DesignationName, "DesignationName", ExcelStyleIndex.WrappedTextStyle),
                new ReportCell(UserExcelColumns.ProgramPartnerName, "ProgramPartnerName", ExcelStyleIndex.WrappedTextStyle),
                new ReportCell(UserExcelColumns.ImplementingPartnerName, "ImplementingPartnerName", ExcelStyleIndex.WrappedTextStyle),
                new ReportCell(UserExcelColumns.EduPartnerName, "EduPartnerName", ExcelStyleIndex.WrappedTextStyle),
                new ReportCell(UserExcelColumns.Email, "Email", ExcelStyleIndex.WrappedTextStyle),
                new ReportCell(UserExcelColumns.RoleName, "RoleName", ExcelStyleIndex.WrappedTextStyle),
                new ReportCell(UserExcelColumns.PhoneNumber, "PhoneNumber", ExcelStyleIndex.WrappedTextStyle)
            };
        }

    }
}