using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ANLASH.Authorization.Roles;
using ANLASH.Authorization.Users;
using ANLASH.MultiTenancy;
using ANLASH.Universities;

namespace ANLASH.EntityFrameworkCore
{
    public class ANLASHDbContext : AbpZeroDbContext<Tenant, Role, User, ANLASHDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        /// <summary>
        /// الجامعات - Universities
        /// </summary>
        public DbSet<University> Universities { get; set; }
        
        public ANLASHDbContext(DbContextOptions<ANLASHDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Universities indexes
            modelBuilder.Entity<University>(entity =>
            {
                // Unique indexes for SEO slugs
                entity.HasIndex(u => u.Slug)
                    .IsUnique()
                    .HasFilter("[Slug] IS NOT NULL");

                entity.HasIndex(u => u.SlugAr)
                    .IsUnique()
                    .HasFilter("[SlugAr] IS NOT NULL");

                // Non-unique indexes for filtering
                entity.HasIndex(u => u.Country);
                entity.HasIndex(u => u.City);
                entity.HasIndex(u => u.Rating);
                entity.HasIndex(u => u.IsActive);
                entity.HasIndex(u => u.IsFeatured);

                // Configure decimal precision for Rating
                entity.Property(u => u.Rating)
                    .HasPrecision(3, 2); // Max value: 5.00
            });
        }
    }
}
