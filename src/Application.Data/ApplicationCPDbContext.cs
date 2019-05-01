using Microsoft.EntityFrameworkCore;
using Application.Entities;

namespace Application.Data
{
    public class ApplicationCPDbContext : DbContext
    {
        public ApplicationCPDbContext(DbContextOptions<ApplicationCPDbContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        
        public DbSet<UserGroups> UserGroups { get; set; }

        public DbSet<Test> Test { get; set; }

        public DbSet<TestType> TestType { get; set; }

        public DbSet<AthleteTestMapping> AthleteTestMapping { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            new ApplicationModelBuilder().BuildModel(builder);
            //builder.Entity<SystemSettings>().HasKey(table => new { table.SettingsName, table.PropertyName });
        }
    }
}
