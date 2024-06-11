using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class Block : BaseModel<int>
    {
        public Block()
        {
            SubBlocks = new HashSet<SubBlock>();
          //  Beneficiaries = new HashSet<Beneficiary>();
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public int CampId { get; set; }
        public Camp Camp { get; set; }
        public ICollection<SubBlock> SubBlocks { get; set; }
    }
}
