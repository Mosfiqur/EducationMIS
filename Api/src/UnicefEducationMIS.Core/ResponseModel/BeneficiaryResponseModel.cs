using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.ResponseModel
{
    public class BeneficiaryResponseModel
    {
        public List<BenecifiaryData> BenecifiaryData { get; set; }
        public int TotalNumber { get; set; }

    }
    public class BenecifiaryData
    {
        public long EntityId { get; set; }
        public List<BeneficiaryCellValue> BeneficiaryCellValues { get; set; }
    }
    public class BeneficiaryCellValue
    {
        public long EntityColumnId { get; set; }
        public string CloumnName { get; set; }
        public string ColumnValue { get; set; }
    }
}
