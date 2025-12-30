using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ANLASH.Universities
{
    /// <summary>
    /// University FAQ Entity
    /// كيان الأسئلة الشائعة للجامعة
    /// </summary>
    [Table("UniversityFAQs")]
    public class UniversityFAQ : FullAuditedEntity<long>
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
        /// </summary>
        [Required]
        [MaxLength(2000)]
        public string Answer { get; set; }
        
        /// <summary>
        /// Answer in Arabic | الإجابة بالعربية
        /// </summary>
        [Required]
        [MaxLength(2000)]
        public string AnswerAr { get; set; }

        #endregion

        #region Display & Status | العرض والحالة

        /// <summary>
        /// Display order for sorting | ترتيب العرض للفرز
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// Is FAQ published? | هل السؤال منشور؟
        /// </summary>
        public bool IsPublished { get; set; } = true;

        #endregion

        // Note: Audit fields are NOW inherited from FullAuditedEntity<long> ✅
        // ملاحظة: حقول التتبع موروثة الآن من FullAuditedEntity<long> ✅
        // - CreationTime (DateTime)
        // - CreatorUserId (long?)
        // - LastModificationTime (DateTime?)
        // - LastModifierUserId (long?)
        // - DeletionTime (DateTime?)
        // - DeleterUserId (long?)
        // - IsDeleted (bool)
    }
}
