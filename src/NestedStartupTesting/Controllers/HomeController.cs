using Microsoft.AspNet.Mvc;

namespace NestedStartupTesting.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
