using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using AspNetSamples.Mvc.Filters;

namespace AspNetSamples.Mvc.Controllers
{
    //[Route("Main")]
    //[MyCustomResourceFilter]
    //[TypeFilter(typeof(MyCustomResourceFilter))]
    [ServiceFilter(typeof(MyCustomResourceFilter))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        //[Route("index")]
        [MyCustomActionFilter]
        public IActionResult Index(string artCount)
        {
            return View();
        }

        [MyCustomActionFilter]
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