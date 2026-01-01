using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ANLASH.LanguageCenters
{
    /// <summary>
    /// دورة لغوية - Language Course Entity
    /// <para>Represents a language course offered by a language center</para>
    /// <para>Supports bilingual content and comprehensive course details</para>
    /// </summary>
    [Table("LanguageCourses")]
    public class LanguageCourse : FullAuditedEntity<long>
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

        #region Course Info (Bilingual) | معلومات الدورة (ثنائي اللغة)

        /// <summary>
        /// Course name in English | اسم الدورة بالإنجليزية
        /// <para>Example: "General English", "IELTS Preparation"</para>
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string CourseName { get; set; }

        /// <summary>
        /// Course name in Arabic | اسم الدورة بالعربية
        /// </summary>
        [Required]
        [MaxLength(300)]
        public string CourseNameAr { get; set; }

        /// <summary>
        /// Course description in English | وصف الدورة بالإنجليزية
        /// </summary>
        [MaxLength(2000)]
        public string Description { get; set; }

        /// <summary>
        /// Course description in Arabic | وصف الدورة بالعربية
        /// </summary>
        [MaxLength(2000)]
        public string DescriptionAr { get; set; }

        #endregion

        #region Course Classification | تصنيف الدورة

        /// <summary>
        /// Course type (General, Academic, Business, etc.) | نوع الدورة
        /// </summary>
        [Required]
        public CourseType CourseType { get; set; }

        /// <summary>
        /// Course level (Beginner, Intermediate, Advanced, etc.) | مستوى الدورة
        /// <para>Based on CEFR standards (A1-C2)</para>
        /// </summary>
        [Required]
        public CourseLevel Level { get; set; }

        #endregion

        #region Age Requirements | متطلبات العمر

        /// <summary>
        /// Minimum age requirement | الحد الأدنى للعمر
        /// <para>Null = No minimum age restriction</para>
        /// </summary>
        public int? MinAge { get; set; }

        /// <summary>
        /// Maximum age requirement | الحد الأقصى للعمر
        /// <para>Null = No maximum age restriction</para>
        /// </summary>
        public int? MaxAge { get; set; }

        #endregion

        #region Class Details | تفاصيل الصف

        /// <summary>
        /// Maximum class size | الحد الأقصى لحجم الصف
        /// <para>Maximum number of students per class</para>
        /// </summary>
        public int? ClassSize { get; set; }

        /// <summary>
        /// Average class size | متوسط حجم الصف
        /// <para>Typical number of students per class</para>
        /// </summary>
        public int? AverageClassSize { get; set; }

        /// <summary>
        /// Hours per week | عدد الساعات الأسبوعية
        /// <para>Total teaching hours per week</para>
        /// </summary>
        public int? HoursPerWeek { get; set; }

        /// <summary>
        /// Lessons per week | عدد الدروس الأسبوعية
        /// <para>Number of lessons/sessions per week</para>
        /// </summary>
        public int? LessonsPerWeek { get; set; }

        /// <summary>
        /// Duration of each lesson in minutes | مدة كل درس بالدقائق
        /// <para>Typically 45, 50, or 60 minutes</para>
        /// </summary>
        public int? LessonDurationMinutes { get; set; }

        #endregion

        #region Schedule | الجدول الزمني

        /// <summary>
        /// Available start dates | تواريخ البدء المتاحة
        /// <para>Comma-separated dates or "Every Monday", "Monthly", etc.</para>
        /// </summary>
        [MaxLength(500)]
        public string StartDates { get; set; }

        /// <summary>
        /// Minimum duration in weeks | الحد الأدنى للمدة بالأسابيع
        /// <para>Shortest course duration available</para>
        /// </summary>
        public int? MinDurationWeeks { get; set; }

        /// <summary>
        /// Maximum duration in weeks | الحد الأقصى للمدة بالأسابيع
        /// <para>Longest course duration available</para>
        /// </summary>
        public int? MaxDurationWeeks { get; set; }

        #endregion

        #region Course Features | مميزات الدورة

        /// <summary>
        /// Includes materials? | هل تشمل المواد الدراسية؟
        /// </summary>
        public bool IncludesMaterials { get; set; }

        /// <summary>
        /// Includes certificate? | هل تشمل شهادة؟
        /// </summary>
        public bool IncludesCertificate { get; set; }

        /// <summary>
        /// Certificate type/name | نوع/اسم الشهادة
        /// <para>E.g., "Certificate of Completion", "IELTS Certificate"</para>
        /// </summary>
        [MaxLength(200)]
        public string CertificateType { get; set; }

        /// <summary>
        /// Includes placement test? | هل تشمل اختبار تحديد المستوى؟
        /// </summary>
        public bool IncludesPlacementTest { get; set; }

        #endregion

        #region Additional Info | معلومات إضافية

        /// <summary>
        /// Course highlights in English | نقاط مميزة عن الدورة بالإنجليزية
        /// <para>JSON array or bullet points</para>
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Highlights { get; set; }

        /// <summary>
        /// Course highlights in Arabic | نقاط مميزة عن الدورة بالعربية
        /// </summary>
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string HighlightsAr { get; set; }

        /// <summary>
        /// Target audience in English | الفئة المستهدفة بالإنجليزية
        /// <para>E.g., "Professionals", "Students", "Travelers"</para>
        /// </summary>
        [MaxLength(500)]
        public string TargetAudience { get; set; }

        /// <summary>
        /// Target audience in Arabic | الفئة المستهدفة بالعربية
        /// </summary>
        [MaxLength(500)]
        public string TargetAudienceAr { get; set; }

        #endregion

        #region Status & Display | الحالة والعرض

        /// <summary>
        /// Is course active? | هل الدورة نشطة؟
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Is course featured/popular? | هل الدورة مميزة/شائعة؟
        /// </summary>
        public bool IsFeatured { get; set; }

        /// <summary>
        /// Display order for sorting | ترتيب العرض للفرز
        /// </summary>
        public int DisplayOrder { get; set; }

        #endregion

        // Note: Audit fields inherited from FullAuditedEntity<long>:
        // - CreationTime, CreatorUserId
        // - LastModificationTime, LastModifierUserId
        // - DeletionTime, DeleterUserId, IsDeleted
    }
}
