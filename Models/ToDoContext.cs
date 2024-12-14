using Microsoft.EntityFrameworkCore;

namespace To_Do_List_App.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext(DbContextOptions<ToDoContext> options): base(options) { }

        public DbSet<ToDo> ToDo { get; set; }= null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Status> Statuss { get; set; }= null!;
      //  public object ToDos { get; internal set; } 

        //seed data

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasData(
                  new Category { CategoryId = "call", Name = "Contact" },
                   new Category { CategoryId = "exercise", Name = "Exercise" },
                  new Category { CategoryId = "home", Name = "Home" },
                  new Category { CategoryId = "shop", Name = "Shopping" },
                   new Category { CategoryId = "work", Name = "Work" }
                 
                );

            modelBuilder.Entity<Status>().HasData(
                new Status { StatusId = "open", Name = "Open" },
                new Status { StatusId = "closed", Name = "Completed" }
                );
        }
    }
}
