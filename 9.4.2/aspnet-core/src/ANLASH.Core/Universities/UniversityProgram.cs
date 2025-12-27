using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using ANLASH.Lookups;

namespace ANLASH.Universities
{
    /// <summary>
    /// University Program Entity
    /// كيان البرنامج الدراسي للجامعة
    /// </summary>
    [Table("UniversityPrograms")]
    public class UniversityProgram : FullAuditedEntity<long>
    {
        #region Relationship | العلاقة

        /// <summary>
        /// University ID | معرف الجامعة
        /// </summary>
        [Required]
        public long UniversityId { get; set; }
        
        /// <summary>
        /// Navigation property to University | خاصية التنقل للجامعة
        /// </summary>
        [ForeignKey(nameof(UniversityId))]
        public virtual University University { get; set; }

        #endregion

        #region Program Info (Bilingual) | معلومات البرنامج (ثنائي اللغة)

        /// <summary>
        /// Program name in English | اسم البرنامج بالإنجليزية
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }
        
        /// <summary>
        /// Program name in Arabic | اسم البرنامج بالعربية
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string NameAr { get; set; }
        
        /// <summary>
        /// Program description in English | وصف البرنامج بالإنجليزية
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; }
        
        /// <summary>
        /// Program description in Arabic | وصف البرنامج بالعربية
        /// </summary>
        [MaxLength(2000)]
        public string DescriptionAr { get; set; }

        #endregion

        #region Program Classification | تصنيف البرنامج

        /// <summary>
        /// Program level (Bachelor, Master, PhD, etc.) | مستوى البرنامج
        /// </summary>
        [Required]
        public ProgramLevel Level { get; set; }
        
        /// <summary>
        /// Study mode (Full-time, Part-time, Online, etc.) | نمط الدراسة
        /// </summary>
        [Required]
        public StudyMode Mode { get; set; }
        
        /// <summary>
        /// Field of study in English | مجال الدراسة بالإنجليزية
        /// </summary>
        [MaxLength(200)]
        public string Field { get; set; }
        
        /// <summary>
        /// Field of study in Arabic | مجال الدراسة بالعربية
        /// </summary>
        [MaxLength(200)]
        public string FieldAr { get; set; }

        #endregion

        #region Duration | المدة الزمنية

        /// <summary>
        /// Program duration in years | مدة البرنامج بالسنوات
        /// </summary>
        public int DurationYears { get; set; }
        
        /// <summary>
        /// Program duration in semesters (optional) | مدة البرنامج بالفصول الدراسية
        /// </summary>
        public int? DurationSemesters { get; set; }
        
        /// <summary>
        /// Program duration in months (optional) | مدة البرنامج بالأشهر
        /// </summary>
        public int? DurationMonths { get; set; }

        #endregion

        #region Credits | الساعات المعتمدة

        /// <summary>
        /// Total credit hours required | إجمالي الساعات المعتمدة المطلوبة
        /// </summary>
        public int? TotalCredits { get; set; }

        #endregion

        #region Tuition Fees | الرسوم الدراسية

        /// <summary>
        /// Tuition fee amount | مبلغ الرسوم الدراسية
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TuitionFee { get; set; }
        
        /// <summary>
        /// Currency ID for tuition fee | معرف العملة للرسوم الدراسية
        /// </summary>
        public int? CurrencyId { get; set; }
        
        /// <summary>
        /// Navigation property to Currency | خاصية التنقل للعملة
        /// </summary>
        [ForeignKey(nameof(CurrencyId))]
        public virtual Currency Currency { get; set; }
        
        /// <summary>
        /// Fee type (Per Year, Per Semester, Total) | نوع الرسوم
        /// </summary>
        [MaxLength(50)]
        public string FeeType { get; set; }

        #endregion

        #region Application | التقديم

        /// <summary>
        /// Application fee | رسوم التقديم
        /// </summary>
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ApplicationFee { get; set; }
        
        /// <summary>
        /// Application deadline | آخر موعد للتقديم
        /// </summary>
        public DateTime? ApplicationDeadline { get; set; }

        #endregion

        #region Requirements | المتطلبات

        /// <summary>
        /// Admission requirements in English | متطلبات القبول بالإنجليزية
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Requirements { get; set; }
        
        /// <summary>
        /// Admission requirements in Arabic | متطلبات القبول بالعربية
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string RequirementsAr { get; set; }

        #endregion

        #region Status & Display | الحالة والعرض

        /// <summary>
        /// Is program active? | هل البرنامج نشط؟
        /// </summary>
        public bool IsActive { get; set; } = true;
        
        /// <summary>
        /// Is program featured? | هل البرنامج مميز؟
        /// </summary>
        public bool IsFeatured { get; set; }
        
        /// <summary>
        /// Display order for sorting | ترتيب العرض للفرز
        /// </summary>
        public int DisplayOrder { get; set; }

        #endregion

        #region SEO | تحسين محركات البحث

        /// <summary>
        /// URL-friendly slug in English | Slug للـ URL بالإنجليزية
        /// </summary>
        [MaxLength(400)]
        public string Slug { get; set; }
        
        /// <summary>
        /// URL-friendly slug in Arabic | Slug للـ URL بالعربية
        /// </summary>
        [MaxLength(400)]
        public string SlugAr { get; set; }

        #endregion

        // Note: Audit fields inherited from FullAuditedEntity<long>:
        // - CreationTime, CreatorUserId
        // - LastModificationTime, LastModifierUserId
        // - DeletionTime, DeleterUserId, IsDeleted
    }
}
