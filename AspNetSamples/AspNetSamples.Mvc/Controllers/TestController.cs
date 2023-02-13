using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.Mvc.Controllers;

//[NonController]
public class TestController : Controller
{
    public IActionResult Index()
    {
        //Bad Practices
        ViewData["SomeKey1"] = 1; //ViewData - same for all app but different from user tu user
        ViewData["SomeKey2"] = "Some string message";
        ViewData["SomeKey3"] = new {A = 1, B=2};
        
        //ViewBag - same bad practice. Never really used
        ViewBag.SomeData = 1234;
        ViewBag.SomeOtherData = "123";


        return View();
    }

    //[NonAction]
    [HttpGet]
    public IActionResult Do()
    {
        return View();
    }


  
    [HttpPost]
    public IActionResult Do(SendDataUsingFormSampleModel model)
    {
        var x = 0;

        return Ok();
    }
    
}