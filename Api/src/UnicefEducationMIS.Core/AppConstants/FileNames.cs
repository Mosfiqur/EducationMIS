using System;
using System.IO;

namespace UnicefEducationMIS.Core.AppConstants
{
    public class FileNames
    {
        public const string EMAIL_TEMPLATE_FOLDER = "EmailTemplates";
        public const string NEW_USER_EMAIL_TEMPLATE = "NEW_USER_EMAIL.txt";
        public static readonly string ImportFolderName = "ImportTemplates";
        public static readonly string BeneficiaryImportTemplateName = "BeneficiaryImportTemplate.xlsx";
        public static readonly string FacilityImportTemplate = "FacilityImportTemplate.xlsx";
        public static readonly string FacilityVersionedImportTemplate = "FacilityVersionedImportTemplate.xlsx";

        public const string ReportTemplateFolder = "ReportTemplates";
        public static readonly string PasswordResetEmailTemplate = "PASSWORD_RESET_EMAIL.txt";
        public const string FiveWTemplateFile = "5wReportTemplate.xlsx";
        public const string CampWiseReportTemplateFile = "CampWiseReportTemplate.xlsx";
        public const string GlobalTemplateFile = "GlobalTemplate.xlsx";
        public const string ContentTypeExcel = "application/vnd.ms-excel";
        public static readonly string BeneficiaryExportsFilename = "Beneficiaries.xlsx";
        public static readonly string FacilityExportsFilename = "Facilities.xlsx";
        public static readonly string FacilityAggExportsFilename = "AggregatedFacilities.xlsx";
        public static readonly string BeneficiaryAggExportsFilename = "AggregatedBeneficiaries.xlsx";

        public static string DuplicationReportFilename = "DuplicationReport.xlsx";
        public static string GapAnalysisReportFilename = "GapAnalysisReport.xlsx";
        public static string DamageReportFilename = "DamageReport.xlsx";
        public static string SummaryReportFilename = "SummaryReport.xlsx";

        public static readonly string FiveWReport = "FiveWReport_"+DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss")+".xlsx";
        public static readonly string CampWiseReport = "CampWiseReport_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";
        public static string FilteredUsersExportFilename = "FilteredUsers.xlsx";
    }
}
