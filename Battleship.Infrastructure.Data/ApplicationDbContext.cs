using Battleship.Domain.Core;
using Microsoft.EntityFrameworkCore;

namespace Battleship.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        
        public DbSet<Player> Players { get; set; }
       
        public DbSet<Game> Games { get; set; }
       
        public DbSet<Field> Fields { get; set; }
        
        public DbSet<Cell> Cells { get; set; }
       
        public DbSet<Ship> Ships { get; set; }

        public DbSet<Step> Steps { get; set; }
        
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }

        public ApplicationDbContext()
        {
            Database.EnsureCreated();
        }
    }
}