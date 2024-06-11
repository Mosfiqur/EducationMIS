using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ValueObjects
{
    public enum CollectionStatus
    {
        NotCollected = 1,
        Collected = 2,
        Approved = 3,
        Recollect = 4,

        Deleted = 5,
        Requested_Inactive = 6,
        Inactivated = 7

    }
}
