using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDoList.Models;

namespace ToDoList
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public virtual DbSet<ToDoItem> ToDoItems { get; set; }
        public virtual DbSet<ItemCategory> Categories { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }
    }
}
