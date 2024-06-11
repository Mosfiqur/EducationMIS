using System;
using System.Collections.Generic;
using UnicefEducationMIS.Core.Models.Framework;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.Models
{
    public partial class EntityDynamicColumn: BaseModel<long>
    {
        public EntityDynamicColumn()
        {
            //Indicator = new HashSet<Indicator>();
            DynamicCell = new HashSet<BeneficiaryDynamicCell>();
            BeneficiaryViewDetails = new HashSet<GridViewDetails>();
            InstanceIndicators = new HashSet<InstanceIndicator>();
            FacilityDynamicCells = new HashSet<FacilityDynamicCell>();
            BudgetFrameworkDynamicCells = new HashSet<BudgetFrameworkDynamicCell>();
            JrpParameterInfos = new HashSet<JrpParameterInfo>();
        }

        public string Name { get; set; }
        public string NameInBangla { get; set; }
        public EntityType EntityTypeId { get; set; }
        public ColumnDataType ColumnType { get; set; }
        public string Unit { get; set; }
        public bool IsFixed { get; set; }
        public bool? IsMultiValued { get; set; }
        public long? ColumnListId { get; set; }
        public virtual ListDataType ColumnList { get; set; }

        public string Description { get; set; }

        public TargetedPopulation? TargetedPopulation { get; set; }

        public bool? IsAutoCalculated { get; set; }
        public bool? IsDeleted { get; set; }


        // public virtual ICollection<Indicator> Indicator { get; set; }
        public virtual ICollection<InstanceIndicator> InstanceIndicators { get; set; }
        public virtual ICollection<BeneficiaryDynamicCell> DynamicCell { get; set; }
        public virtual ICollection<GridViewDetails> BeneficiaryViewDetails { get; set; }
        public virtual ICollection<FacilityDynamicCell> FacilityDynamicCells { get; set; }
        public ICollection<BudgetFrameworkDynamicCell> BudgetFrameworkDynamicCells { get; set; }
        public ICollection<TargetFrameworkDynamicCell> TargetFrameworkDynamicCells { get; set; }
        public ICollection<JrpParameterInfo> JrpParameterInfos { get; set; }
    }
}
