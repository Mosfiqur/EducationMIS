using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Core.Exceptions
{
    public class InstanceHasNoIndicatorException : DomainException
    {
        public InstanceHasNoIndicatorException(string msg = Messages.NoInstanceIndicatorFound) : base(msg)
        {
        }

        public override int ToHttpStatusCode()
        {
            return AppStatusCode.BadRequestStatusCode;
        }
    }
}
