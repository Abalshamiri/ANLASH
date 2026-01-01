using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ANLASH.LanguageCenters
{
    /// <summary>
    /// Language Center FAQ Entity
    /// كيان الأسئلة الشائعة للمعهد اللغوي
    /// <para>Frequently asked questions for language centers with bilingual support</para>
    /// </summary>
    [Table("LanguageCenterFAQs")]
    public class LanguageCenterFAQ : FullAuditedEntity<long>
    {
        #region Relationship | العلاقة

        /// <summary>
        /// Language Center ID | معرف المعهد اللغوي
        /// </summary>
        [Required]
        public long LanguageCenterId { get; set; }

        /// <summary>
        /// Navigation property to Language Center | خاصية التنقل للمعهد
        /// </summary>
        [ForeignKey(nameof(LanguageCenterId))]
        public virtual LanguageCenter LanguageCenter { get; set; }

        #endregion

        #region Question (Bilingual) | السؤال (ثنائي اللغة)

        /// <summary>
        /// Question in English | السؤال بالإنجليزية
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Question { get; set; }

        /// <summary>
        /// Question in Arabic | السؤال بالعربية
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string QuestionAr { get; set; }

        #endregion

        #region Answer (Bilingual) | الإجابة (ثنائية اللغة)

        /// <summary>
        /// Answer in English | الإجابة بالإنجليزية
        /// <para>Supports rich HTML content</para>
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Answer { get; set; }

        /// <summary>
        /// Answer in Arabic | الإجابة بالعربية
        /// <para>Supports rich HTML content</para>
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string AnswerAr { get; set; }

        #endregion

        #region Categorization | التصنيف

        /// <summary>
        /// FAQ category in English | فئة السؤال بالإنجليزية
        /// <para>E.g., "Admission", "Courses", "Accommodation", "Visa"</para>
        /// </summary>
        [MaxLength(100)]
        public string Category { get; set; }

        /// <summary>
        /// FAQ category in Arabic | فئة السؤال بالعربية
        /// </summary>
        [MaxLength(100)]
        public string CategoryAr { get; set; }

        #endregion

        #region Display & Status | العرض والحالة

        /// <summary>
        /// Display order for sorting | ترتيب العرض للفرز
        /// <para>Lower numbers appear first</para>
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Is FAQ published? | هل السؤال منشور؟
        /// <para>Only published FAQs are visible to public</para>
        /// </summary>
        public bool IsPublished { get; set; } = true;

        /// <summary>
        /// Is this a featured/important FAQ? | هل هذا سؤال مميز/مهم؟
        /// <para>Featured FAQs can be highlighted or shown first</para>
        /// </summary>
        public bool IsFeatured { get; set; }

        #endregion

        #region Analytics | التحليلات

        /// <summary>
        /// View count | عدد المشاهدات
        /// <para>Tracks how many times this FAQ was viewed</para>
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// Helpful count | عدد مرات التقييم بـ "مفيد"
        /// <para>Number of users who found this FAQ helpful</para>
        /// </summary>
        public int HelpfulCount { get; set; }

        /// <summary>
        /// Not helpful count | عدد مرات التقييم بـ "غير مفيد"
        /// <para>Number of users who found this FAQ not helpful</para>
        /// </summary>
        public int NotHelpfulCount { get; set; }

        #endregion

        // Note: Audit fields are inherited from FullAuditedEntity<long>:
        // ملاحظة: حقول التتبع موروثة من FullAuditedEntity<long>:
        // - CreationTime (DateTime)
        // - CreatorUserId (long?)
        // - LastModificationTime (DateTime?)
        // - LastModifierUserId (long?)
        // - DeletionTime (DateTime?)
        // - DeleterUserId (long?)
        // - IsDeleted (bool)
    }
}
