using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public class IndicatorRawView
    {
        public long BeneficiaryId { get; set; }
        public string BeneficairyName { get; set; }
        public long FacilityId { get; set; }
        public string FacilityName { get; set; }

        public long InstanceId { get; set; }


        public DateTime DataCollectionDate { get; set; }
        public long DataCollectionStatusId { get; set; }
        public CollectionStatus CollectionStatus { get; set; }
        public long PropertiesId { get; set; }
        public string PropertiesValue { get; set; }


        public long EntityColumnId { get; set; }
        public string EntityColumnName { get; set; }
        public string EntityColumnNameInBangla { get; set; }
        public int ColumnOrder { get; set; }


        public bool? IsMultiValued { get; set; }
        public ColumnDataType DataType { get; set; }
        public long? ColumnListId { get; set; }
        public string ColumnListName { get; set; }

        public long ListItemId { get; set; }
        public string ListItemTitle { get; set; }
        public int ListItemValue { get; set; }
    }
}
