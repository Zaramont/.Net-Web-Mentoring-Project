using Microsoft.AspNetCore.Http;

namespace MyCatalogSite.Models
{
    public class FileUploadModel
    {
        public IFormFile FormFile { get; set; }
    }
}
