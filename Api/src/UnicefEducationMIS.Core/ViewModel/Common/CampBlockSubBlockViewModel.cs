using System.Collections.Generic;

namespace UnicefEducationMIS.Core.ViewModel.Common
{
    public class CampBlockSubBlockViewModel
    {
        public CampViewModel Camp { get; set; }
        //public int CampId { get; set; }
        //public string CampSSID { get; set; }
        //public string CampName { get; set; }
        //public string CampNameAlias { get; set; }
        //public int CampUnionId { get; set; }
        //public CampViewModel Camp { get; set; }
        public List<BlockViewModel> Blocks { get; set; }
        public List<SubBlockViewModel> SubBlocks { get; set; }
    }
}
