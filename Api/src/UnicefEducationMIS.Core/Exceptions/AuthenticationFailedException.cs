using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Core.Exceptions
{
    public class AuthenticationFailedException : DomainException
    {
        public AuthenticationFailedException(string msg) : base(msg)
        {
        }

        public override int ToHttpStatusCode()
        {
            return AppStatusCode.UnAuthorized;
        }
    }
}
