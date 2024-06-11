using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core
{
    public interface IAuditable
    {
        DateTime DateCreated { get; set; }
        int CreatedBy { get; set; }
        DateTime? LastUpdated { get; set; }
        int? UpdatedBy { get; set; }
    }
}
