using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Core.Exceptions
{
    public class InstanceAlreadyRunningException : DomainException
    {
        public InstanceAlreadyRunningException() : base(string.Format(Messages.InstanceAlreadyRunning))
        {

        }

        public override int ToHttpStatusCode()
        {
            return AppStatusCode.BadRequestStatusCode;
        }
    }
}