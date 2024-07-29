using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.Api.Controllers
{
    [Route("api/v{version:apiVersion}/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider _fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            _fileExtensionContentTypeProvider = fileExtensionContentTypeProvider;
        }

        [HttpGet("{fileid}")]
        [ApiVersion(0.1, Deprecated = true)]
        public ActionResult GetFile(string fileId) 
        {
            var file = "getting-started-with-rest-slides.pdf";
            
            if (!System.IO.File.Exists(file))
            {
                return NotFound();
            }

            if(!_fileExtensionContentTypeProvider.TryGetContentType(file, out var contentType))
            {
                // default for binary data
                contentType = "appplication/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(file);
            return File(bytes,contentType, Path.GetFileName(file));
        }

        [HttpPost]
        public async Task<ActionResult> CreateFile(IFormFile file)
        {
            if (file.Length == 0 || file.Length > 20971520 || file.ContentType != "application/pdf") 
            {
                return BadRequest("No file or an invalid input was provided.");

            }

            // Create the file path.  Avoid using file.FileName, as an attacker can provide a
            // malicious one, including full paths or relative paths.
            // also avoid creating it in the directory api itself and put it in a location without execution previledges.
            // we never know we might get in there.
            var path = Path.Combine(
                Directory.GetCurrentDirectory(),
                $"uploaded_file_{Guid.NewGuid()}.pdf");

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Ok("Your file has been uploaded successfully.");
        }
    }
}
