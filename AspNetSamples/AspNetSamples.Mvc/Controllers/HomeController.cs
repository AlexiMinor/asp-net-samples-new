using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AspNetSamples.Mvc.Controllers
{
    //[Route("Main")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //[Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("Privacy/general")]
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private void Do(int[] arr)
        {
            foreach (var i in arr)
            {
                Console.WriteLine(i);
            }
        }

        private void Do(IEnumerable<int> arr)
        {
            foreach (var i in arr)
            {
                Console.WriteLine(i);
            }
        }
    }
}