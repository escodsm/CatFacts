using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CatFacts.Services;

namespace CatFacts.Controllers
{
    public class HomeController : Controller
    {
        private readonly CatFactService _catFactService;

        public HomeController(CatFactService catFactService)
        {
            _catFactService = catFactService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var catFacts = await _catFactService.FetchAndProcessCatFacts();
                return View(catFacts);
            }
            catch
            {
                ViewBag.ErrorMessage = "Failed to load cat facts. Check logs for details.";
                return View(new List<CatFacts.Models.CatFact>());
            }
        }
    }
}