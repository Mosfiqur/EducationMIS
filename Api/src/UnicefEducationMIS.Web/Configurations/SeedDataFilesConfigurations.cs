using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnicefEducationMIS.Web.Configurations
{
    public class SeedDataFilesConfigurations
    {
        public string DataRoot { get; set; }
        public string UsersFilename { get; set; }
        public string RolesFilename { get; set; }
        public string DesignationsFilename { get; set; }
        public string UserLevelsFilename { get; set; }
        public string PermissionsFilename { get; set; }
        public string PermissionPresetsFilename { get; set; }
        public string EducationSectorPartnerFilename { get; set; }
        public string DistrictFileName { get; set; }
        public string UpazilaFileName { get; set; }
        public string CampFileName { get; set; }
        public string UnionFileName { get; set; }
        public string BlockFileName { get; set; }
        public string SubBlockFileName { get; set; }
        public string CampCoordinateFileName { get; set; }
        public string ReportingFrequencyFileName { get; set; }
        public string AgeGroupFileName { get; set; }
        public string EntityDynamicColumnFileName { get; set; }
    }
}
