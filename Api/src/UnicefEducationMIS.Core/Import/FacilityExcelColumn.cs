using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Import
{
    public class FacilityExcelColumn : ExcelCell
    {
        
        public FacilityExcelColumn(string displayName, string actualName):base(displayName.ToLower(), actualName, string.Empty, false)
        {            
        }
        public FacilityExcelColumn(string displayName, string actualName, uint style) : base(displayName.ToLower(), actualName, style)
        {
        }
        public FacilityExcelColumn(string displayName, string actualName, string value) : base(displayName.ToLower(), actualName, value, false)
        {
        }
        public FacilityExcelColumn(string displayName, string actualName, bool isMandatory) : base(displayName.ToLower(), actualName, string.Empty, isMandatory)
        {
        }
        public FacilityExcelColumn(string displayName, string actualName, bool isMandatory,bool isAutoCalculated) : base(displayName.ToLower(), actualName, string.Empty, isMandatory,isAutoCalculated)
        {
            
        }
        public FacilityExcelColumn(string displayName, string actualName, string value, bool isMandatory) : base(displayName.ToLower(), actualName, value, isMandatory)
        {
        }
    }
}
