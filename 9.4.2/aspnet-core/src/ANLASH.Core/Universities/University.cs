using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ANLASH.Universities
{
    /// <summary>
    /// جامعة - University Entity
    /// <para>Core entity representing a university with bilingual support (AR/EN)</para>
    /// </summary>
    [Table("Universities")]
    public class University : FullAuditedEntity<int>
    {
        #region English Fields

        /// <summary>
        /// اسم الجامعة بالإنجليزية - University name in English
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// الوصف بالإنجليزية - Description in English
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; }

        #endregion

        #region Arabic Fields ✨

        /// <summary>
        /// اسم الجامعة بالعربية - University name in Arabic
        /// </summary>
        [MaxLength(200)]
        public string NameAr { get; set; }

        /// <summary>
        /// الوصف بالعربية - Description in Arabic
        /// </summary>
        [MaxLength(2000)]
        public string DescriptionAr { get; set; }

        #endregion

        #region Location Fields

        /// <summary>
        /// الدولة - Country
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        /// <summary>
        /// المدينة - City
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        /// <summary>
        /// العنوان - Address
        /// </summary>
        [MaxLength(500)]
        public string Address { get; set; }

        #endregion

        #region Additional Information

        /// <summary>
        /// نوع الجامعة - University Type (Public/Private)
        /// </summary>
        public UniversityType Type { get; set; }

        /// <summary>
        /// شعار الجامعة - University Logo URL
        /// </summary>
        [MaxLength(500)]
        public string LogoUrl { get; set; }

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
        public decimal Rating { get; set; }

        /// <summary>
        /// تصنيف الجامعة عالمياً - World Ranking
        /// </summary>
        public int? WorldRanking { get; set; }

        /// <summary>
        /// سنة التأسيس - Establishment Year
        /// </summary>
        public int? EstablishmentYear { get; set; }

        #endregion

        #region SEO & Display

        /// <summary>
        /// الرابط الودي بالإنجليزية - SEO-friendly URL slug (English)
        /// </summary>
        [MaxLength(300)]
        public string Slug { get; set; }

        /// <summary>
        /// الرابط الودي بالعربية - SEO-friendly URL slug (Arabic)
        /// </summary>
        [MaxLength(300)]
        public string SlugAr { get; set; }

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

        // FullAuditedEntity<int> provides:
        // - Id (int primary key)
        // - CreationTime, CreatorUserId
        // - LastModificationTime, LastModifierUserId
        // - DeletionTime, DeleterUserId, IsDeleted (Soft Delete)
    }
}
