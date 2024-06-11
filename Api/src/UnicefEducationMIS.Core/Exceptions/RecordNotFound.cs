using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Core.Exceptions
{
    public class RecordNotFound : DomainException
    {
        public RecordNotFound(string msg = Messages.RecordNotFound) : base(msg) { }
        public override int ToHttpStatusCode()
        {
            return AppStatusCode.NotFound;
        }
    }
}
