using AwesomeBooks.Domain.Entities;
using AwesomeBooks.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AwesomeBooks.Domain.EF
{
    public class DomainContext : IdentityDbContext<User, Role, long, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public DomainContext() : base() { }

        public DomainContext(DbContextOptions<DomainContext> options) : base(options)
        {
        }

        public DbSet<AppSetting> AppSettings { get; set; }

        public DbSet<CategoryArea> CategoryAreas { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Identity
            builder.Entity<User>().ToTable("User");
            builder.Entity<Role>().ToTable("Role");
            builder.Entity<UserClaim>().ToTable("UserClaim");
            builder.Entity<UserRole>().ToTable("UserRole");
            builder.Entity<UserLogin>().ToTable("UserLogin");
            builder.Entity<RoleClaim>().ToTable("RoleClaim");
            builder.Entity<UserToken>().ToTable("UserToken");

            builder.Entity<User>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<User>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // App
            builder.Entity<AppSetting>().ToTable("AppSetting");
            var bookTable = builder.Entity<Book>().ToTable("Book");
            bookTable.HasIndex(b => b.Name);
            bookTable.HasIndex(b => b.PublishYear);
            bookTable.HasIndex(b => b.Authors);
            builder.Entity<CategoryArea>().ToTable("CategoryArea").HasIndex(ca => ca.Name);
            builder.Entity<Category>().ToTable("Category").HasIndex(c => c.Name);
        }
    }
}
