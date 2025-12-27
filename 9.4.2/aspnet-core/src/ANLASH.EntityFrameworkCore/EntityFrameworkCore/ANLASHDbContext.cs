using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ANLASH.Authorization.Roles;
using ANLASH.Authorization.Users;
using ANLASH.MultiTenancy;
using ANLASH.Universities;
using ANLASH.Lookups;
using ANLASH.Storage;

namespace ANLASH.EntityFrameworkCore
{
    public class ANLASHDbContext : AbpZeroDbContext<Tenant, Role, User, ANLASHDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
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

        /// <summary>
        /// الجامعات - Universities
        /// </summary>
        public DbSet<University> Universities { get; set; }

        /// <summary>
        /// محتويات الجامعات - University Contents
        /// </summary>
        public DbSet<UniversityContent> UniversityContents { get; set; }

        /// <summary>
        /// برامج الجامعات - University Programs
        /// </summary>
        public DbSet<UniversityProgram> UniversityPrograms { get; set; }

        /// <summary>
        /// الملفات المخزنة - Stored Files
        /// </summary>
        public DbSet<AppBinaryObject> AppBinaryObjects { get; set; }
        
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

            // ✅ Configure University Entity (Enhanced)
            modelBuilder.Entity<University>(entity =>
            {
                entity.ToTable("Universities");
                
                // Primary Key (Changed from int to long)
                entity.HasKey(u => u.Id);
                
                // ✅ Foreign Key Relationships
                
                // Country relationship (optional)
                entity.HasOne(u => u.Country)
                    .WithMany()
                    .HasForeignKey(u => u.CountryId)
                    .OnDelete(DeleteBehavior.SetNull);  // Keep university if country is deleted
                
                // City relationship (optional)
                entity.HasOne(u => u.City)
                    .WithMany()
                    .HasForeignKey(u => u.CityId)
                    .OnDelete(DeleteBehavior.SetNull);  // Keep university if city is deleted
                
                // ✅ Required Fields
                entity.Property(u => u.Name)
                    .IsRequired()
                    .HasMaxLength(300);
                    
                entity.Property(u => u.NameAr)
                    .IsRequired()
                    .HasMaxLength(300);
                
                // ✅ Rich Content Fields (NVARCHAR(MAX))
                entity.Property(u => u.AboutText)
                    .HasColumnType("NVARCHAR(MAX)");
                    
                entity.Property(u => u.AboutTextAr)
                    .HasColumnType("NVARCHAR(MAX)");
                
                // ✅ Decimal Fields Precision
                entity.Property(u => u.Rating)
                    .HasPrecision(3, 2);  // 0.00 to 5.00
                    
                entity.Property(u => u.OfferLetterFee)
                    .HasPrecision(18, 2);  // Up to 999,999,999,999,999.99
                
                // ✅ Unique Indexes for SEO Slugs
                entity.HasIndex(u => u.Slug)
                    .IsUnique()
                    .HasDatabaseName("IX_Universities_Slug")
                    .HasFilter("[Slug] IS NOT NULL");

                entity.HasIndex(u => u.SlugAr)
                    .IsUnique()
                    .HasDatabaseName("IX_Universities_SlugAr")
                    .HasFilter("[SlugAr] IS NOT NULL");

                // ✅ Non-Unique Indexes for Filtering & Performance
                entity.HasIndex(u => u.CountryId)
                    .HasDatabaseName("IX_Universities_CountryId");
                    
                entity.HasIndex(u => u.CityId)
                    .HasDatabaseName("IX_Universities_CityId");
                    
                entity.HasIndex(u => u.Rating)
                    .HasDatabaseName("IX_Universities_Rating");
                    
                entity.HasIndex(u => u.IsActive)
                    .HasDatabaseName("IX_Universities_IsActive");
                    
                entity.HasIndex(u => u.IsFeatured)
                    .HasDatabaseName("IX_Universities_IsFeatured");
                
                entity.HasIndex(u => u.TenantId)
                    .HasDatabaseName("IX_Universities_TenantId");
            });

            #endregion

            #region Configure UniversityContent Entity

            modelBuilder.Entity<UniversityContent>(entity =>
            {
                entity.ToTable("UniversityContents");

                // Primary Key
                entity.HasKey(c => c.Id);

                // Foreign Key to University
                entity.HasOne(c => c.University)
                    .WithMany(u => u.Contents)
                    .HasForeignKey(c => c.UniversityId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Required Fields
                entity.Property(c => c.Title).IsRequired().HasMaxLength(200);
                entity.Property(c => c.TitleAr).IsRequired().HasMaxLength(200);
                entity.Property(c => c.Content).IsRequired().HasColumnType("NVARCHAR(MAX)");
                entity.Property(c => c.ContentAr).IsRequired().HasColumnType("NVARCHAR(MAX)");
                entity.Property(c => c.ContentType).IsRequired();

                // Default Values
                entity.Property(c => c.IsActive).HasDefaultValue(true);
                entity.Property(c => c.DisplayOrder).HasDefaultValue(0);

                // Indexes for Performance
                entity.HasIndex(c => c.UniversityId).HasDatabaseName("IX_UniversityContents_UniversityId");
                entity.HasIndex(c => c.ContentType).HasDatabaseName("IX_UniversityContents_ContentType");
                entity.HasIndex(c => c.DisplayOrder).HasDatabaseName("IX_UniversityContents_DisplayOrder");
                entity.HasIndex(c => c.IsActive).HasDatabaseName("IX_UniversityContents_IsActive");

                // Unique Constraint: One content type per university
                entity.HasIndex(c => new { c.UniversityId, c.ContentType })
                    .IsUnique()
                    .HasDatabaseName("UQ_UniversityContents_UniversityId_ContentType")
                    .HasFilter("[IsDeleted] = 0");
            });

            #endregion

            #region Configure UniversityProgram Entity

            modelBuilder.Entity<UniversityProgram>(entity =>
            {
                entity.ToTable("UniversityPrograms");

                // Primary Key
                entity.HasKey(p => p.Id);

                // Foreign Keys
                entity.HasOne(p => p.University)
                    .WithMany(u => u.Programs)
                    .HasForeignKey(p => p.UniversityId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(p => p.Currency)
                    .WithMany()
                    .HasForeignKey(p => p.CurrencyId)
                    .OnDelete(DeleteBehavior.SetNull);

                // Required Fields
                entity.Property(p => p.Name).IsRequired().HasMaxLength(300);
                entity.Property(p => p.NameAr).IsRequired().HasMaxLength(300);

                // Rich Content
                entity.Property(p => p.Requirements).HasColumnType("NVARCHAR(MAX)");
                entity.Property(p => p.RequirementsAr).HasColumnType("NVARCHAR(MAX)");

                // Decimal Columns
                entity.Property(p => p.TuitionFee).HasColumnType("decimal(18,2)");
                entity.Property(p => p.ApplicationFee).HasColumnType("decimal(18,2)");

                // Default Values
                entity.Property(p => p.IsActive).HasDefaultValue(true);
                entity.Property(p => p.IsFeatured).HasDefaultValue(false);

                // Indexes
                entity.HasIndex(p => p.UniversityId).HasDatabaseName("IX_UniversityPrograms_UniversityId");
                entity.HasIndex(p => p.Level).HasDatabaseName("IX_UniversityPrograms_Level");
                entity.HasIndex(p => p.Mode).HasDatabaseName("IX_UniversityPrograms_Mode");
                entity.HasIndex(p => p.IsActive).HasDatabaseName("IX_UniversityPrograms_IsActive");
                entity.HasIndex(p => p.IsFeatured).HasDatabaseName("IX_UniversityPrograms_IsFeatured");

                // Unique Slugs
                entity.HasIndex(p => p.Slug).IsUnique().HasDatabaseName("UQ_UniversityPrograms_Slug")
                    .HasFilter("[Slug] IS NOT NULL AND [IsDeleted] = 0");
                entity.HasIndex(p => p.SlugAr).IsUnique().HasDatabaseName("UQ_UniversityPrograms_SlugAr")
                    .HasFilter("[SlugAr] IS NOT NULL AND [IsDeleted] = 0");
            });

            #endregion

            #region Configure AppBinaryObject Entity

            modelBuilder.Entity<AppBinaryObject>(entity =>
            {
                entity.ToTable("AppBinaryObjects");

                // Primary Key
                entity.HasKey(b => b.Id);

                // Required Fields
                entity.Property(b => b.FileName).IsRequired().HasMaxLength(500);
                entity.Property(b => b.ContentType).IsRequired().HasMaxLength(100);
                entity.Property(b => b.FileSize).IsRequired();
                entity.Property(b => b.Bytes).IsRequired();

                // Optional Fields
                entity.Property(b => b.Description).HasMaxLength(500);
                entity.Property(b => b.Category).HasMaxLength(100);
                entity.Property(b => b.EntityType).HasMaxLength(100);

                // Indexes for Performance
                entity.HasIndex(b => b.TenantId).HasDatabaseName("IX_AppBinaryObjects_TenantId");
                entity.HasIndex(b => b.Category).HasDatabaseName("IX_AppBinaryObjects_Category");
                entity.HasIndex(b => new { b.EntityType, b.EntityId }).HasDatabaseName("IX_AppBinaryObjects_Entity");
                entity.HasIndex(b => b.CreationTime).HasDatabaseName("IX_AppBinaryObjects_CreationTime");
            });

            #endregion
        }
    }
}
