using System.Collections.Generic;
using UnicefEducationMIS.Core.ViewModel;

namespace UnicefEducationMIS.Core.ResponseModel
{
    public class LoginResponse
    {
        public string Token { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
        public List<string> Permissions { get; set; }
        public UserViewModel UserProfile { get; set; }
    }
}
