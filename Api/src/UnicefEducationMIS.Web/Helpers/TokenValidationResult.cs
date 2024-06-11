using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnicefEducationMIS.Web.Helpers
{
    public class TokenValidationResult
    {
        public bool IsValid { get; set; }

        public static TokenValidationResult ValidResult() => new TokenValidationResult { IsValid = true };
        public static TokenValidationResult InValidResult() => new TokenValidationResult { IsValid = false };
    }
}
