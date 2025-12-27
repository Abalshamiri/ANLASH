using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ANLASH.Authorization.Roles;
using ANLASH.Authorization.Users;
using ANLASH.MultiTenancy;
using ANLASH.Universities;
using ANLASH.Lookups;

namespace ANLASH.EntityFrameworkCore
{
    public class ANLASHDbContext : AbpZeroDbContext<Tenant, Role, User, ANLASHDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        /// <summary>
        /// الجامعات - Universities
        /// </summary>
        public DbSet<University> Universities { get; set; }
        
        /// <summary>
        /// العملات - Currencies
        /// </summary>
        public DbSet<Currency> Currencies { get; set; }
        
        /// <summary>
        /// الدول - Countries
        /// </summary>
        public DbSet<Country> Countries { get; set; }
        
        /// <summary>
        /// المدن - Cities
        /// </summary>
        public DbSet<City> Cities { get; set; }
        
        public ANLASHDbContext(DbContextOptions<ANLASHDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Configure Lookup Entities

            // ✅ Currency Configuration
            modelBuilder.Entity<Currency>(entity =>
            {
                entity.ToTable("Currencies");
                
                // Primary Key
                entity.HasKey(c => c.Id);
                
                // Unique index on Code
                entity.HasIndex(c => c.Code)
                    .IsUnique()
                    .HasDatabaseName("IX_Currencies_Code");
                
                // Required fields
                entity.Property(c => c.Code)
                    .IsRequired()
                    .HasMaxLength(10);
                    
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(c => c.NameAr)
                    .IsRequired()
                    .HasMaxLength(100);
                    
                entity.Property(c => c.Symbol)
                    .HasMaxLength(10);
            });

            // ✅ Country Configuration
            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("Countries");
                
                // Primary Key
                entity.HasKey(c => c.Id);
                
                // Unique index on Code
                entity.HasIndex(c => c.Code)
                    .IsUnique()
                    .HasDatabaseName("IX_Countries_Code");
                
                // Index for filtering
                entity.HasIndex(c => c.IsActive);
                
                // Required fields
                entity.Property(c => c.Code)
                    .IsRequired()
                    .HasMaxLength(3);
                    
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                    
                entity.Property(c => c.NameAr)
                    .IsRequired()
                    .HasMaxLength(200);
            });

            // ✅ City Configuration
            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("Cities");
                
                // Primary Key
                entity.HasKey(c => c.Id);
                
                // Composite index on CountryId and Name for filtering
                entity.HasIndex(c => new { c.CountryId, c.Name })
                    .HasDatabaseName("IX_Cities_CountryId_Name");
                
                // Index for filtering
                entity.HasIndex(c => c.IsActive);
                
                // Required fields
                entity.Property(c => c.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                    
                entity.Property(c => c.NameAr)
                    .IsRequired()
                    .HasMaxLength(200);
                
                // Foreign Key Relationship with Country
                entity.HasOne(c => c.Country)
                    .WithMany(co => co.Cities)
                    .HasForeignKey(c => c.CountryId)
                    .OnDelete(DeleteBehavior.Restrict); // Prevent deleting country if it has cities
            });

            #endregion

            #region Configure Universities

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

            #endregion
        }
    }
}
