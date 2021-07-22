using Aspose.Imaging.Cloud.Live.Demos.Models;
using Aspose.Imaging.Cloud.Live.Demos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.IO;
using System.Threading.Tasks;


namespace Aspose.Imaging.Cloud.Live.Demos.Controllers
{
    public class ConversionController : BaseController
    {
        public ConversionController(IMemoryCache cache, IAsposeDiagramCloudApiService diagramService) : base(cache)
        {
            DiagramService = diagramService;
        }

        public override string Product => (string)RouteData.Values["product"];

        public IAsposeDiagramCloudApiService DiagramService { get; }

        public IActionResult Conversion()
        {
            var model = new ViewModel(this, nameof(Conversion))
            {
                SaveAsComponent = true,
                MaximumUploadFiles = 1,
                UseSorting = false
            };

            return View(model);
        }

        [HttpPost]
        public async Task<Response> Conversion(string outputType)
        {
            IFormFile postedFile = Request.Form.Files[0];
            string fileName = postedFile.FileName;
            string fileData = string.Empty;
            string outputFileName = string.Empty;

            using (MemoryStream ms = new MemoryStream())
            {
                await postedFile.CopyToAsync(ms);
                ms.Position = 0;

                var file = ms;
                outputFileName = Path.GetFileNameWithoutExtension(fileName) + "." + outputType;

                using (MemoryStream convertResult = DiagramService.Convert(file, fileName, outputFileName) as MemoryStream)
                {
                    fileData = Convert.ToHexString(convertResult.ToArray());
                }
            }

            return new Response
            {
                StatusCode = 200,
                FileName = outputFileName,
                FileData = fileData
            };
        }

    }
}
