using Aspose.Diagram.Cloud.Live.Demos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Aspose.Diagram.Cloud.Live.Demos.Controllers
{
    public class HomeController : BaseController
    {
        public override string Product => string.Empty;

        public HomeController(IMemoryCache cache) : base(cache)
        {
        }

        public IActionResult Default()
        {
            ViewBag.PageTitle = Resources["diagramConversionPageTitle"];
            ViewBag.MetaDescription = Resources["diagramConversionMetaDescription"];

            var model = new LandingPageModel(this);
            return View(model);
        }
    }
}
