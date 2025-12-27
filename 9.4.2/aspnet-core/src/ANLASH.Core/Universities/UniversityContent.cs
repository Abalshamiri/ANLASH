using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ANLASH.Universities
{
    /// <summary>
    /// University Content Entity
    /// كيان محتوى الجامعة - يسمح بإضافة sections مختلفة من المحتوى لكل جامعة
    /// </summary>
    public class UniversityContent : FullAuditedEntity<long>
    {
        // ===== Relationship | العلاقة =====
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
        
        // ===== Content Type | نوع المحتوى =====
        /// <summary>
        /// Type of content (Overview, Admissions, etc.) | نوع المحتوى
        /// </summary>
        [Required]
        public UniversityContentType ContentType { get; set; }
        
        // ===== Title (Bilingual) | العنوان (ثنائي اللغة) =====
        /// <summary>
        /// Content title in English | عنوان المحتوى بالإنجليزية
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }
        
        /// <summary>
        /// Content title in Arabic | عنوان المحتوى بالعربية
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string TitleAr { get; set; }
        
        // ===== Content (Rich HTML) | المحتوى (HTML غني) =====
        /// <summary>
        /// Rich HTML content in English | محتوى HTML غني بالإنجليزية
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Content { get; set; }
        
        /// <summary>
        /// Rich HTML content in Arabic | محتوى HTML غني بالعربية
        /// </summary>
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string ContentAr { get; set; }
        
        // ===== Display Order | ترتيب العرض =====
        /// <summary>
        /// Display order for sorting | ترتيب العرض للفرز
        /// </summary>
        public int DisplayOrder { get; set; }
        
        // ===== Status | الحالة =====
        /// <summary>
        /// Is content active? | هل المحتوى نشط؟
        /// </summary>
        public bool IsActive { get; set; } = true;
        
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
