using Microsoft.EntityFrameworkCore;
using TaskFlow.Models;

namespace TaskFlow.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<TaskFlow.Models.Task> Tasks { get; set; }
        public DbSet<TaskFlow.Models.Project> Projects { get; set; }
        public DbSet<TaskFlow.Models.User> Users { get; set; }
    }
}
