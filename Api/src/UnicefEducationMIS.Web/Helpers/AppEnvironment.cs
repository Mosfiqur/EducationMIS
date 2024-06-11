using Microsoft.AspNetCore.Hosting;
using UnicefEducationMIS.Core.Interfaces;

namespace UnicefEducationMIS.Web.Helpers
{
    public class AppEnvironment:IEnvironment
    {
        private readonly IWebHostEnvironment _hostEnv;

        public AppEnvironment(IWebHostEnvironment hostEnv)
        {
            _hostEnv = hostEnv;
        }    

        public string GetRootPath()
        {
            return _hostEnv.WebRootPath; 
        }
    }
}
