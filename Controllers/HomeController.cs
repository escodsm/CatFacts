using Microsoft.AspNetCore.Mvc;

namespace CatFacts.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("It works!");
        }
    }
}