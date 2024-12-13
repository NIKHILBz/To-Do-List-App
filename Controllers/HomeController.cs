using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Statuses = context.Statuss.ToList();
            ViewBag.DueFilters = Filters.DueFilterValues;

            IQueryable<ToDo> query = context.ToDo.Include(t => t.Category).Include(t => t.Status);

            if (filters.HasCategory)
            {
                query = query.Where(t => t.CategoryId == filters.CatergoryId);
            }

            if (filters.HasStatus)
            {
                query = query.Where(t => t.StatusId == filters.Statusid);

            }

            if (filters.HasDue)
            {
              var today = DateTime.Today;
                if (filters.IsPast)
                {
                    query = query.Where(today => t.DueDate < today);
                }

            }

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
