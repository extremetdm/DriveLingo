using System.Data.Entity;
using DriveLingo.Database.Models;

namespace DriveLingo.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
    }
}