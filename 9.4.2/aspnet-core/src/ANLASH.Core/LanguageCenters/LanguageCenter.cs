using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ANLASH.Lookups;

namespace ANLASH.LanguageCenters
{
    /// <summary>
    /// معهد لغة - Language Center Entity
    /// <para>Core entity representing a language center with bilingual support (AR/EN)</para>
    /// <para>Enhanced with lookup-based location, SEO, blob storage, and accreditation support</para>
    /// </summary>
    [Table("LanguageCenters")]
    public class LanguageCenter : FullAuditedAggregateRoot<long>
    {
        #region Basic Information - المعلومات الأساسية

        /// <summary>
        /// اسم المعهد بالإنجليزية - Language center name in English
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        /// <summary>
        /// اسم المعهد بالعربية - Language center name in Arabic
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string NameAr { get; set; }

        /// <summary>
        /// الوصف المختصر بالإنجليزية - Short description in English
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// الوصف المختصر بالعربية - Short description in Arabic
        /// </summary>
        [MaxLength(2000)]
        public string DescriptionAr { get; set; }

        #endregion

        #region Rich Content - المحتوى الغني

        /// <summary>
        /// محتوى تفصيلي عن المعهد بالإنجليزية - Rich HTML content about the center
        /// <para>Supports CKEditor rich text with images, tables, etc.</para>
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string AboutText { get; set; }

        /// <summary>
        /// محتوى تفصيلي عن المعهد بالعربية - Rich HTML content in Arabic
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string AboutTextAr { get; set; }

        #endregion

        #region Location - Lookup Based

        /// <summary>
        /// معرّف الدولة - Country ID (Foreign Key)
        /// </summary>
        public int? CountryId { get; set; }

        /// <summary>
        /// الدولة - Country (Navigation Property)
        /// </summary>
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }

        /// <summary>
        /// معرّف المدينة - City ID (Foreign Key)
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// المدينة - City (Navigation Property)
        /// </summary>
        [ForeignKey(nameof(CityId))]
        public virtual City City { get; set; }

        /// <summary>
        /// الولاية/المقاطعة - State/Province
        /// <para>For countries with state-level divisions (e.g., USA, Australia)</para>
        /// </summary>
        [MaxLength(100)]
        public string State { get; set; }

        /// <summary>
        /// العنوان التفصيلي - Detailed Address
        /// </summary>
        [MaxLength(500)]
        public string Address { get; set; }

        /// <summary>
        /// خط العرض - Latitude for map integration
        /// </summary>
        [Column(TypeName = "decimal(10,8)")]
        public decimal? Latitude { get; set; }

        /// <summary>
        /// خط الطول - Longitude for map integration
        /// </summary>
        [Column(TypeName = "decimal(11,8)")]
        public decimal? Longitude { get; set; }

        #endregion

        #region Contact Information - معلومات الاتصال

        /// <summary>
        /// الموقع الإلكتروني - Website URL
        /// </summary>
        [MaxLength(500)]
        public string WebsiteUrl { get; set; }

        /// <summary>
        /// البريد الإلكتروني - Email
        /// </summary>
        [MaxLength(200)]
        public string Email { get; set; }

        /// <summary>
        /// رقم الهاتف - Phone Number
        /// </summary>
        [MaxLength(50)]
        public string Phone { get; set; }

        /// <summary>
        /// رقم الواتساب - WhatsApp Number
        /// <para>Separate from phone for direct WhatsApp integration</para>
        /// </summary>
        [MaxLength(50)]
        public string WhatsApp { get; set; }

        #endregion

        #region Media - Blob Storage

        /// <summary>
        /// معرّف شعار المعهد في Blob Storage - Logo Blob ID
        /// <para>Reference to ABP Blob Storage</para>
        /// </summary>
        public Guid? LogoBlobId { get; set; }

        /// <summary>
        /// [DEPRECATED] رابط الشعار القديم - Legacy Logo URL
        /// <para>Use LogoBlobId instead</para>
        /// </summary>
        [MaxLength(500)]
        public string LogoUrl { get; set; }

        /// <summary>
        /// معرّف صورة الغلاف في Blob Storage - Cover Image Blob ID
        /// </summary>
        public Guid? CoverImageBlobId { get; set; }

        /// <summary>
        /// معرض الصور - Gallery Images (JSON array of Blob IDs)
        /// <para>Stored as JSON array: ["guid1", "guid2", ...]</para>
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string GalleryImages { get; set; }

        #endregion

        #region Accreditation - الاعتماد

        /// <summary>
        /// هل المعهد معتمد؟ - Is center accredited?
        /// </summary>
        public bool IsAccredited { get; set; }

        /// <summary>
        /// جهة الاعتماد - Accreditation body/organization
        /// <para>E.g., "British Council", "ACCET", "CEA"</para>
        /// </summary>
        [MaxLength(200)]
        public string AccreditationBody { get; set; }

        /// <summary>
        /// رقم الاعتماد - Accreditation number/certificate
        /// </summary>
        [MaxLength(100)]
        public string AccreditationNumber { get; set; }

        /// <summary>
        /// تاريخ انتهاء الاعتماد - Accreditation expiry date
        /// </summary>
        public DateTime? AccreditationExpiryDate { get; set; }

        #endregion

        #region Registration Process - عملية التسجيل

        /// <summary>
        /// خطوات التسجيل - Registration steps (JSON array)
        /// <para>Stored as JSON: [{"step": 1, "title": "...", "description": "..."}, ...]</para>
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string RegistrationSteps { get; set; }

        /// <summary>
        /// المستندات المطلوبة - Required documents (JSON array)
        /// <para>Stored as JSON: ["Passport copy", "Photo", "Application form", ...]</para>
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string RequiredDocuments { get; set; }

        #endregion

        #region Accommodation - السكن

        /// <summary>
        /// هل يوفر المعهد سكن؟ - Does center provide accommodation?
        /// </summary>
        public bool ProvidesAccommodation { get; set; }

        /// <summary>
        /// أنواع السكن المتاحة - Available accommodation types (JSON)
        /// <para>E.g., Homestay, Student Residence, Apartment</para>
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string AccommodationTypes { get; set; }

        /// <summary>
        /// تفاصيل السكن بالإنجليزية - Accommodation details in English
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string AccommodationDetails { get; set; }

        /// <summary>
        /// تفاصيل السكن بالعربية - Accommodation details in Arabic
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string AccommodationDetailsAr { get; set; }

        #endregion

        #region SEO Optimization

        /// <summary>
        /// الرابط الودي بالإنجليزية - SEO-friendly URL slug (English)
        /// <para>Example: "british-council-dubai"</para>
        /// </summary>
        [MaxLength(400)]
        public string Slug { get; set; }

        /// <summary>
        /// الرابط الودي بالعربية - SEO-friendly URL slug (Arabic)
        /// <para>Example: "المجلس-الثقافي-البريطاني-دبي"</para>
        /// </summary>
        [MaxLength(400)]
        public string SlugAr { get; set; }

        /// <summary>
        /// وصف meta للمحركات البحث بالإنجليزية - Meta description for SEO (English)
        /// </summary>
        [MaxLength(500)]
        public string MetaDescription { get; set; }

        /// <summary>
        /// وصف meta للمحركات البحث بالعربية - Meta description for SEO (Arabic)
        /// </summary>
        [MaxLength(500)]
        public string MetaDescriptionAr { get; set; }

        #endregion

        #region Display & Status

        /// <summary>
        /// معهد نشط - Is Active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// معهد مميز - Is Featured (for homepage, etc.)
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// تقييم المعهد - Center Rating (0-5)
        /// </summary>
        [Range(0, 5)]
        [Column(TypeName = "decimal(3,2)")]
        public decimal Rating { get; set; }

        /// <summary>
        /// ترتيب العرض - Display Order
        /// </summary>
        public int DisplayOrder { get; set; }

        #endregion

        #region Multi-Tenancy

        /// <summary>
        /// معرّف المستأجر - Tenant ID (for SaaS support)
        /// <para>Null = Host data, Not Null = Tenant-specific data</para>
        /// </summary>
        public int? TenantId { get; set; }

        #endregion

        // ✅ FullAuditedAggregateRoot<long> provides:
        // - Id (long primary key)
        // - CreationTime, CreatorUserId
        // - LastModificationTime, LastModifierUserId  
        // - DeletionTime, DeleterUserId, IsDeleted (Soft Delete)
        
        // ✅ IMultiTenant provides:
        // - TenantId (int?) - for multi-tenancy support
    }
}
