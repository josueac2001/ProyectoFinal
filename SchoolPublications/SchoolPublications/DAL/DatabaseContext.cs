
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolPublications.DAL.Entities;
namespace SchoolPublications.DAL
{
    public class DatabaseContext : IdentityDbContext<User>

    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Publication> Publications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Publication>().HasIndex("Id", "CategoryId").IsUnique();
            modelBuilder.Entity<Comment>().HasIndex("Id", "PublicationId").IsUnique();
        }

    }
}
