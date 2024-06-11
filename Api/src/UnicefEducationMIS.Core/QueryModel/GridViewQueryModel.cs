using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.QueryModel
{
    public class GridViewQueryModel:BaseQueryModel
    {
        public EntityType EntityTypeId { get; set; }
    }
}
