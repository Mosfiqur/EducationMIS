using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Core.Exceptions
{
    public class AuthException : DomainException
    {
        public AuthException(string msg) : base(msg)
        {

        }

        public override int ToHttpStatusCode()
        {
            return AppStatusCode.Forbidden;
        }
    }
}
