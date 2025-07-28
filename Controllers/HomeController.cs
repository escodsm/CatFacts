using CatFacts.Models;
using CatFacts.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                var facts = await _catFactService.FetchAndProcessCatFacts();
                return View(facts);
            }
            catch
            {
                ViewBag.ErrorMessage = "Failed to load cat facts.";
                return View(new List<CatFact>()); // show error in view
            }
        }
    }
}
