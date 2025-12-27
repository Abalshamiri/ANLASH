using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using ANLASH.Lookups;

namespace ANLASH.Universities
{
    /// <summary>
    /// جامعة - University Entity
    /// <para>Core entity representing a university with bilingual support (AR/EN)</para>
    /// <para>Enhanced with lookup-based location, SEO, and blob storage support</para>
    /// </summary>
    [Table("Universities")]
    public class University : FullAuditedAggregateRoot<long>
    {
        #region Basic Information - المعلومات الأساسية

        /// <summary>
        /// اسم الجامعة بالإنجليزية - University name in English
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        /// <summary>
        /// اسم الجامعة بالعربية - University name in Arabic
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

        #region Rich Content - المحتوى الغني ✨ NEW!

        /// <summary>
        /// محتوى تفصيلي عن الجامعة بالإنجليزية - Rich HTML content about the university
        /// <para>Supports CKEditor rich text with images, tables, etc.</para>
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string AboutText { get; set; }

        /// <summary>
        /// محتوى تفصيلي عن الجامعة بالعربية - Rich HTML content in Arabic
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string AboutTextAr { get; set; }

        #endregion

        #region Location - Lookup Based ✨ NEW!

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
        /// العنوان التفصيلي - Detailed Address
        /// </summary>
        [MaxLength(500)]
        public string Address { get; set; }

        #endregion

        #region OLD Location Fields - للتوافق المؤقت (سيتم حذفها لاحقاً)

        /// <summary>
        /// [DEPRECATED] الدولة (نص) - استخدم CountryId بدلاً منها
        /// <para>Will be removed after data migration</para>
        /// </summary>
        [MaxLength(100)]
        [NotMapped] // لن نستخدمها في قاعدة البيانات الآن
        public string Country_Old { get; set; }

        /// <summary>
        /// [DEPRECATED] المدينة (نص) - استخدم CityId بدلاً منها
        /// <para>Will be removed after data migration</para>
        /// </summary>
        [MaxLength(100)]
        [NotMapped]
        public string City_Old { get; set; }

        #endregion

        #region University Details - تفاصيل الجامعة

        /// <summary>
        /// نوع الجامعة - University Type (Public/Private/NonProfit)
        /// </summary>
        public UniversityType Type { get; set; }

        /// <summary>
        /// الموقع الإلكتروني - Website URL
        /// </summary>
        [MaxLength(300)]
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
        /// تقييم الجامعة - University Rating (0-5)
        /// </summary>
        [Range(0, 5)]
        [Column(TypeName = "decimal(3,2)")]
        public decimal Rating { get; set; }

        /// <summary>
        /// التصنيف العالمي - World Ranking
        /// </summary>
        public int? WorldRanking { get; set; }

        /// <summary>
        /// سنة التأسيس - Establishment Year
        /// </summary>
        public int? EstablishmentYear { get; set; }

        #endregion

        #region Media - Blob Storage ✨ NEW!

        /// <summary>
        /// معرّف شعار الجامعة في Blob Storage - Logo Blob ID
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

        #endregion

        #region SEO Optimization ✨ NEW!

        /// <summary>
        /// الرابط الودي بالإنجليزية - SEO-friendly URL slug (English)
        /// <para>Example: "king-saud-university"</para>
        /// </summary>
        [MaxLength(400)]
        public string Slug { get; set; }

        /// <summary>
        /// الرابط الودي بالعربية - SEO-friendly URL slug (Arabic)
        /// <para>Example: "جامعة-الملك-سعود"</para>
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

        #region Metadata ✨ NEW!

        /// <summary>
        /// رسوم إصدار خطاب القبول - Offer Letter Fee
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OfferLetterFee { get; set; }

        /// <summary>
        /// أشهر القبول - Intake Months (comma-separated)
        /// <para>Example: "1,5,9" for January, May, September</para>
        /// </summary>
        [MaxLength(100)]
        public string IntakeMonths { get; set; }

        #endregion

        #region Display & Status

        /// <summary>
        /// جامعة نشطة - Is Active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// جامعة مميزة - Is Featured (for homepage, etc.)
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// ترتيب العرض - Display Order
        /// </summary>
        public int DisplayOrder { get; set; }

        #endregion

        #region Multi-Tenancy ✨ NEW!

        /// <summary>
        /// معرّف المستأجر - Tenant ID (for SaaS support)
        /// <para>Null = Host data, Not Null = Tenant-specific data</para>
        /// </summary>
        public int? TenantId { get; set; }

        #endregion

        #region Navigation Properties - خصائص التنقل

        /// <summary>
        /// University contents collection | مجموعة محتويات الجامعة
        /// </summary>
        public virtual ICollection<UniversityContent> Contents { get; set; }

        /// <summary>
        /// University programs collection | مجموعة برامج الجامعة
        /// </summary>
        public virtual ICollection<UniversityProgram> Programs { get; set; }

        #endregion
        
        // ✅ FullAuditedAggregateRoot<long> provides:
        // - Id (long primary key) ✨ Changed from int to long
        // - CreationTime, CreatorUserId
        // - LastModificationTime, LastModifierUserId  
        // - DeletionTime, DeleterUserId, IsDeleted (Soft Delete)
        
        // ✅ IMultiTenant provides:
        // - TenantId (int?) - for multi-tenancy support
    }
}
