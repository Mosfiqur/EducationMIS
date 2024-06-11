using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace UnicefEducationMIS.Web.ActionResults
{
    public class DocumentResult : IActionResult
    {
        private readonly byte[] _bytes;
        private readonly string _filename;
        public DocumentResult(string path)
        {
            var filePath = path;
            _filename = Path.GetFileName(filePath);
            _bytes = File.ReadAllBytes(filePath);
        }

        public DocumentResult(byte[] bytes, string filename)
        {
            _bytes = bytes;
            _filename = filename;
        }

        public Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");
            context.HttpContext.Response.Headers["content-disposition"] = "attachment; filename=" + _filename;
            return context.HttpContext.Response.Body.WriteAsync(_bytes, 0, _bytes.Length);
        }
    }
}
