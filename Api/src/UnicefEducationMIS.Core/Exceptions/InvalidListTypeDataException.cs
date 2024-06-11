using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Core.Exceptions
{
    public class InvalidListTypeDataException : DomainException
    {
        public InvalidListTypeDataException(string msg = Messages.InvalidListTypeDataException) : base(msg) { }
        public override int ToHttpStatusCode()
        {
            return AppStatusCode.BadRequestStatusCode;
        }
    }
}
