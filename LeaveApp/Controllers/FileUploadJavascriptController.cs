using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace LeaveApp.Controllers
{
    public class FileUploadJavascriptController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Upload(string? str = null)
        {
           

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var file = Request.Form.Files[0];
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/UploadFile", file.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Json(new { success = true });
        }
    }
}
