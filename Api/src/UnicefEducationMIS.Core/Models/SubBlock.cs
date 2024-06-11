using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.Models
{
    public class SubBlock : BaseModel<int>
    {
        public SubBlock()
        {
          //  Beneficiaries = new HashSet<Beneficiary>();
        }
        public string Name { get; set; }
        public int BlockId { get; set; }
        public Block Block { get; set; }

        //public ICollection<Beneficiary> Beneficiaries { get; set; }
    }
}
