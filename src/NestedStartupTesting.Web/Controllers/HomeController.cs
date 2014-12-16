using Microsoft.AspNet.Mvc;

namespace NestedStartupTesting.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
