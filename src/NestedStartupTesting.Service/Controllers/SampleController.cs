using Microsoft.AspNet.Mvc;

namespace NestedStartupTesting.Service.Controllers
{
    public class SampleController
    {
        public IActionResult Index()
        {
            return new JsonResult(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
        }
    }
}