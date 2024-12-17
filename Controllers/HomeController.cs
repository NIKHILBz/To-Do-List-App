using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using To_Do_List_App.Models;

namespace To_Do_List_App.Controllers
{
    public class HomeController : Controller
    {

        private ToDoContext context;
        public HomeController(ToDoContext ctx) =>context = ctx;

        public IActionResult Index(string id, string sortBy, string sortDirection = "asc")
        {
            var filters = new Filters(id);
            ViewBag.Filters = filters;

            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Statuses = context.Statuss.ToList();
            ViewBag.DueFilters = Filters.DueFilterValues;

            IQueryable<ToDo> query = context.ToDo.Include(t => t.Category).Include(t => t.Status);

            // Apply filters
            if (filters.HasCategory)
                query = query.Where(t => t.CategoryId == filters.CategoryId);

            if (filters.HasStatus)
                query = query.Where(t => t.StatusId == filters.StatusId);

            if (filters.HasDue)
            {
                var today = DateTime.Today;
                if (filters.IsPast)
                    query = query.Where(t => t.DueDate < today);
                else if (filters.IsFuture)
                    query = query.Where(t => t.DueDate > today);
                else if (filters.IsToday)
                    query = query.Where(t => t.DueDate == today);
            }

            // Sorting logic
            sortDirection = sortDirection.ToLower() == "desc" ? "desc" : "asc";
            ViewBag.SortDirection = sortDirection;
            ViewBag.CurrentSortBy = sortBy;

            query = sortBy switch
            {
                "Category" => sortDirection == "asc" ? query.OrderBy(t => t.Category.Name) : query.OrderByDescending(t => t.Category.Name),
                "DueDate" => sortDirection == "asc" ? query.OrderBy(t => t.DueDate) : query.OrderByDescending(t => t.DueDate),
                "Status" => sortDirection == "asc" ? query.OrderBy(t => t.Status.Name) : query.OrderByDescending(t => t.Status.Name),
                "Description" => sortDirection == "asc" ? query.OrderBy(t => t.Description) : query.OrderByDescending(t => t.Description),
                _ => query.OrderBy(t => t.DueDate),
            };

            var tasks = query.ToList();
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

        [HttpPost]
        public IActionResult MarkAsOpen(int Id)
        {
            var task = context.ToDo.Find(Id);
            if (task != null)
            {
                task.StatusId = "open"; // Update the status to open
                context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

    }
}
