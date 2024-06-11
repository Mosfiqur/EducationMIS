using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.Models.Identity;
using UnicefEducationMIS.Core.Repositories;

namespace UnicefEducationMIS.Core.Models
{
    public class BaseModel<TId> : IAuditable
    {
        public TId Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int? UpdatedBy { get; set; }

        //public virtual User UserCreated { get; set; }
        //public virtual User UserUpdated { get; set; }
    }
}
