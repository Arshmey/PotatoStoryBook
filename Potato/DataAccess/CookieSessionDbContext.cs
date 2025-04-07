using Microsoft.EntityFrameworkCore;
using Potato.Models;

namespace Potato.DataAccess
{
    public class CookieSessionDbContext : DbContext
    {

        private readonly IConfiguration configuration;

        public DbSet<CookieUser> CookieUsers { get; set; }

        public CookieSessionDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CookieUser>().ToTable("CookieUsers")
                .HasIndex(i => i.CookieID)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Directory.Exists("DataBase"))
            {
                optionsBuilder.UseSqlite("Data Source=DataBase/CookieSessionUsers.db");
            }
            else
            {
                Directory.CreateDirectory("DataBase");
                optionsBuilder.UseSqlite("Data Source=DataBase/CookieSessionUsers.db");
            }
        }

    }
}
