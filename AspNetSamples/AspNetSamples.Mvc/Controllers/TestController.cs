using AspNetSamples.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetSamples.Mvc.Controllers;

//[NonController]
public class TestController : Controller
{
    private IConfiguration _configuration;

    public TestController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

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


    [HttpGet]
    public IActionResult Config()
    {
        var connString = _configuration.GetConnectionString("DefaultConnection");
        var secretKey = _configuration["Secrets:Key"];
        var name1 = _configuration["User1"];
        var name2 = _configuration["User2"];

        var model = new ConfigTestModel()
        {
            ConnectionString = connString,
            SecretKey = secretKey,
            User1 = name1,
            User2 = name2,

        };

        return View(model);
    }
}