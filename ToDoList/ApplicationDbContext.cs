using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList
{
    public class ApplicationDbContext : DbContext
    {
        public virtual DbSet<ToDoItem> ToDoItems { get; set; }
        public virtual DbSet<ItemCategory> Categories { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }
    }
}
