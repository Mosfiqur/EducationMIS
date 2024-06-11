using UnicefEducationMIS.Core.AppConstants;

namespace UnicefEducationMIS.Core.Exceptions
{
    public class InvalidCoordinateException : DomainException
    {
        public InvalidCoordinateException(string msg = Messages.CoordindateIsOutsideOfCamp) : base(msg)
        {
        }
    }
}