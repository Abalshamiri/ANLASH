using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ANLASH.Lookups;

namespace ANLASH.LanguageCenters
{
    /// <summary>
    /// تسعير الدورة - Course Pricing Entity
    /// <para>Represents pricing information for different course durations</para>
    /// <para>Allows flexible pricing based on course length</para>
    /// </summary>
    [Table("CoursePricing")]
    public class CoursePricing : FullAuditedEntity<long>
    {
        #region Relationship | العلاقة

        /// <summary>
        /// Language Course ID | معرف الدورة اللغوية
        /// </summary>
        [Required]
        public long LanguageCourseId { get; set; }

        /// <summary>
        /// Navigation property to Language Course | خاصية التنقل للدورة
        /// </summary>
        [ForeignKey(nameof(LanguageCourseId))]
        public virtual LanguageCourse LanguageCourse { get; set; }

        #endregion

        #region Duration | المدة

        /// <summary>
        /// Course duration in weeks | مدة الدورة بالأسابيع
        /// <para>E.g., 1, 2, 4, 8, 12, 24, 48 weeks</para>
        /// </summary>
        [Required]
        public int DurationWeeks { get; set; }

        #endregion

        #region Pricing | التسعير

        /// <summary>
        /// Course fee amount | مبلغ رسوم الدورة
        /// <para>Total fee for the specified duration</para>
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Fee { get; set; }

        /// <summary>
        /// Currency ID | معرف العملة
        /// </summary>
        [Required]
        public int CurrencyId { get; set; }

        /// <summary>
        /// Navigation property to Currency | خاصية التنقل للعملة
        /// </summary>
        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }

        /// <summary>
        /// Fee per week (calculated) | الرسوم الأسبوعية (محسوبة)
        /// <para>Fee / DurationWeeks - for comparison purposes</para>
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FeePerWeek { get; set; }

        #endregion

        #region Additional Fees | رسوم إضافية

        /// <summary>
        /// One-time registration fee | رسوم التسجيل لمرة واحدة
        /// <para>Charged once at enrollment</para>
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RegistrationFee { get; set; }

        /// <summary>
        /// Materials/books fee | رسوم المواد/الكتب
        /// <para>Cost of course materials and textbooks</para>
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaterialsFee { get; set; }

        /// <summary>
        /// Exam fee (if applicable) | رسوم الامتحان (إن وجدت)
        /// <para>For courses with certification exams</para>
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ExamFee { get; set; }

        #endregion

        #region Visa Information | معلومات الفيزا

        /// <summary>
        /// Visa duration in weeks | مدة الفيزا بالأسابيع
        /// <para>Student visa validity period for this course duration</para>
        /// </summary>
        public int? VisaDurationWeeks { get; set; }

        /// <summary>
        /// Visa processing fee | رسوم معالجة الفيزا
        /// <para>Cost of visa application assistance</para>
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? VisaProcessingFee { get; set; }

        #endregion

        #region Discounts & Promotions | الخصومات والعروض

        /// <summary>
        /// Has discount? | هل يوجد خصم؟
        /// </summary>
        public bool HasDiscount { get; set; }

        /// <summary>
        /// Discount percentage | نسبة الخصم
        /// <para>E.g., 10 for 10% discount</para>
        /// </summary>
        [Column(TypeName = "decimal(5,2)")]
        public decimal? DiscountPercentage { get; set; }

        /// <summary>
        /// Discount amount | مبلغ الخصم
        /// <para>Fixed discount amount</para>
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DiscountAmount { get; set; }

        /// <summary>
        /// Final price after discount | السعر النهائي بعد الخصم
        /// <para>Calculated: Fee - DiscountAmount or Fee * (1 - DiscountPercentage/100)</para>
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? FinalPrice { get; set; }

        /// <summary>
        /// Promotion description in English | وصف العرض بالإنجليزية
        /// <para>E.g., "Early bird discount", "Summer special"</para>
        /// </summary>
        [MaxLength(500)]
        public string PromotionDescription { get; set; }

        /// <summary>
        /// Promotion description in Arabic | وصف العرض بالعربية
        /// </summary>
        [MaxLength(500)]
        public string PromotionDescriptionAr { get; set; }

        #endregion

        #region Status & Display | الحالة والعرض

        /// <summary>
        /// Is pricing active? | هل التسعير نشط؟
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Is this the most popular duration? | هل هذه المدة الأكثر شيوعاً؟
        /// <para>Highlight as "Most Popular" option</para>
        /// </summary>
        public bool IsMostPopular { get; set; }

        /// <summary>
        /// Display order for sorting | ترتيب العرض للفرز
        /// <para>Typically ordered by duration: 1, 2, 4, 8, 12 weeks...</para>
        /// </summary>
        public int DisplayOrder { get; set; }

        #endregion

        #region Notes | ملاحظات

        /// <summary>
        /// Additional notes in English | ملاحظات إضافية بالإنجليزية
        /// <para>Special conditions, requirements, or information</para>
        /// </summary>
        [MaxLength(1000)]
        public string Notes { get; set; }

        /// <summary>
        /// Additional notes in Arabic | ملاحظات إضافية بالعربية
        /// </summary>
        [MaxLength(1000)]
        public string NotesAr { get; set; }

        #endregion

        // Note: Audit fields inherited from FullAuditedEntity<long>:
        // - CreationTime, CreatorUserId
        // - LastModificationTime, LastModifierUserId
        // - DeletionTime, DeleterUserId, IsDeleted
    }
}
