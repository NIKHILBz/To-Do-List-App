using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using To_Do_List_App.Models;

namespace To_Do_List_App.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private ToDoContext context;
        public HomeController(ToDoContext ctx) =>context = ctx;

        public IActionResult Index(string id)
        {
           var filters = new Filters(id);
            ViewBag.Filters = filters;
            return View();
        }

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}
