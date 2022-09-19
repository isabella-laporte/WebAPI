using Microsoft.EntityFrameworkCore;
using WebAPI.Models;

namespace WebAPI.Context
{
    public class MemoryContext : DbContext
    {
        public MemoryContext(DbContextOptions<MemoryContext> options) : base(options)
        { 
        }

        public DbSet<Books> Books { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
    }
}

