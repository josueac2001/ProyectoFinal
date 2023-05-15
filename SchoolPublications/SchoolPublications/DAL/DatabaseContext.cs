
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

        public DbSet<Approval> Aprovals { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Publication> Publications { get; set; }
        //public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Publication>().HasIndex("Id", "CategoryId").IsUnique();
            modelBuilder.Entity<Approval>().HasIndex("Id", "PublicationId").IsUnique();
            modelBuilder.Entity<Comment>().HasIndex("Id", "PublicationId").IsUnique();
            //modelBuilder.Entity<Notification>().HasIndex("Id", "ApprovalId").IsUnique();

        }

    }
}
