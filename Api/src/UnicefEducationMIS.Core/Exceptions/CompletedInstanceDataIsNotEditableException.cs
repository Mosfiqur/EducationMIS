using System;
using System.Collections.Generic;
using System.Text;
using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Core.Exceptions
{
    public class CompletedInstanceDataIsNotEditableException : DomainException
    {
        public CompletedInstanceDataIsNotEditableException(string msg=Messages.CompletedInstanceDataNotEditable) : base(msg)
        {
        }
    }
}
