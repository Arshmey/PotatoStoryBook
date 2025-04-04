using Microsoft.EntityFrameworkCore;
using Potato.Models;

namespace Potato.DataAccess
{
    public class DataDbContext : DbContext
    {

        private readonly IConfiguration configuration;

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DataDbContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Message>().ToTable("Messages");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (Directory.Exists("DataBase"))
            {
                optionsBuilder.UseSqlite("Data Source=DataBase/Data.db");
            }
            else
            {
                Directory.CreateDirectory("DataBase");
                optionsBuilder.UseSqlite("Data Source=DataBase/Data.db");
            }
        }

    }
}
