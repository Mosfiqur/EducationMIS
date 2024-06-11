using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Core.Exceptions
{
    public class EmailAlreadyTakenException : DomainException
    {
        public EmailAlreadyTakenException(string email) : base(string.Format(Messages.EmailAlreadyTaken, email))
        {

        }

        public override int ToHttpStatusCode()
        {
            return AppStatusCode.BadRequestStatusCode;
        }
    }
}
