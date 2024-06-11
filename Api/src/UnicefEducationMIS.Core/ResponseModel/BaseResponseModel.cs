using System;
using System.Collections.Generic;
using System.Text;

namespace UnicefEducationMIS.Core.ResponseModel
{
    public class BaseResponseModel
    {
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsSuccess { get; set; }
    }
}
