using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.ValueObjects;

namespace UnicefEducationMIS.Core.ViewModel
{
    public class GridViewViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public EntityType EntityTypeId { get; set; }
        public List<GridViewDetailsViewModel> GridViewDetails { get; set; }


    }
}
