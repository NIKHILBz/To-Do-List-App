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
                    query = query.Where(t => t.DueDate < today);
                }
                else if (filters.IsFuture)
                {
                    query = query.Where(t => t.DueDate > today);    
                }
                else if (filters.IsToday)
                {
                    query= query.Where(t => t.DueDate == today);
                }
               
            }
            var tasks = query.OrderBy(t => t.DueDate).ToList();
            return View(tasks);
        }
        [HttpGet]

        public IActionResult Add()
        {
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Statuses= context.Statuss.ToList();
            var task = new ToDo { StatusId = "open" };
            return View(task);
        }

        [HttpPost]
        public IActionResult Add(ToDo task)
        {
            if (ModelState.IsValid)
            {
                context.ToDo.Add(task);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Categories = context.Categories.ToList();
                ViewBag.Statuss = context.Statuss.ToList();
                return View(task);
            }
        }

        [HttpPost]
        public IActionResult Filter(string[] filter)
        {
            string id = string.Join('-', filter);
            return RedirectToAction("Index", new {ID= id});
        }

        [HttpPost]
        public IActionResult MarkComplete([FromRoute]string id, ToDo selected)
        {
            selected = context.ToDo.Find(selected.Id)!;

            if (selected != null)
            {
                selected.StatusId = "closed";
                context.SaveChanges();
            }
            return RedirectToAction("Index", new {Id = id});
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
        [HttpPost]
        public IActionResult DeleteComplete(string id)
        {
            var toDelete = context.ToDo.Where(t => t.StatusId == "closed").ToList();

            foreach (var task in toDelete)
            {
                context.ToDo.Remove(task);
            }
            context.SaveChanges();
            return RedirectToAction("Index", new {ID= id});
        }
    }
}
