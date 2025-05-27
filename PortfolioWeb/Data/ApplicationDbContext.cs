using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortfolioWeb.Models;

namespace PortfolioWeb.Data
    {
        public class ApplicationDbContext : IdentityDbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
                : base(options)
            {
            }

            public DbSet<Project> Projects { get; set; }
            public DbSet<Experience> Experiences { get; set; }
            public DbSet<Education> Educations { get; set; }
            public DbSet<ContactForm> ContactForms { get; set; }
            public DbSet<VisitorLog> VisitorLogs { get; set; }
            public DbSet<BlogPost> BlogPosts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BlogPost>(entity =>
            {
                entity.Property(b => b.Title).IsRequired().HasMaxLength(200);
                entity.Property(b => b.Excerpt).HasMaxLength(500);
                entity.Property(b => b.Content).IsRequired();
                entity.Property(b => b.ImagePath).HasMaxLength(255);

                // Configure Author relationship
                entity.HasOne(b => b.Author)
                    .WithMany()
                    .HasForeignKey(b => b.AuthorId)
                    .OnDelete(DeleteBehavior.Restrict); // Or Cascade if you want to delete posts when user is deleted

                entity.Property(b => b.PublishedDate)
                    .HasDefaultValueSql("GETDATE()");

                entity.Property(b => b.IsPublished)
                    .HasDefaultValue(true);

                entity.HasIndex(b => b.PublishedDate);
                entity.HasIndex(b => b.IsPublished);
                entity.HasIndex(b => b.AuthorId);
            });

            builder.Entity<Project>(entity =>
            {
                entity.Property(p => p.Title).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Description).HasMaxLength(500);
                entity.Property(p => p.Technologies).HasMaxLength(200);
                entity.Property(p => p.GitHubUrl).HasMaxLength(200);
                entity.Property(p => p.LiveDemoUrl).HasMaxLength(200);
                entity.Property(p => p.ImagePath).HasMaxLength(255);
                entity.Property(p => p.CreatedDate).HasDefaultValueSql("GETDATE()");
                entity.Property(p => p.IsFeatured).HasDefaultValue(false);

                entity.HasOne<IdentityUser>()
                    .WithMany()
                    .HasForeignKey(p => p.AuthorId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
    }