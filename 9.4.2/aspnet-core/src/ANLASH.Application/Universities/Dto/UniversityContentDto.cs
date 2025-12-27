using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using ANLASH.Universities;

namespace ANLASH.Universities.Dto
{
    /// <summary>
    /// University Content DTO
    /// DTO لمحتوى الجامعة
    /// </summary>
    public class UniversityContentDto : FullAuditedEntityDto<long>
    {
        /// <summary>
        /// University ID | معرف الجامعة
        /// </summary>
        [Required]
        public long UniversityId { get; set; }
        
        /// <summary>
        /// Content type | نوع المحتوى
        /// </summary>
        [Required]
        public UniversityContentType ContentType { get; set; }
        
        /// <summary>
        /// Content type name (from enum Display attribute) | اسم نوع المحتوى
        /// </summary>
        public string ContentTypeName { get; set; }
        
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
        
        /// <summary>
        /// Rich HTML content in English | محتوى HTML غني بالإنجليزية
        /// </summary>
        [Required]
        public string Content { get; set; }
        
        /// <summary>
        /// Rich HTML content in Arabic | محتوى HTML غني بالعربية
        /// </summary>
        [Required]
        public string ContentAr { get; set; }
        
        /// <summary>
        /// Display order for sorting | ترتيب العرض للفرز
        /// </summary>
        public int DisplayOrder { get; set; }
        
        /// <summary>
        /// Is content active? | هل المحتوى نشط؟
        /// </summary>
        public bool IsActive { get; set; }
        
        // Inherited from FullAuditedEntityDto<long>:
        // - Id (long)
        // - CreationTime (DateTime)
        // - CreatorUserId (long?)
        // - LastModificationTime (DateTime?)
        // - LastModifierUserId (long?)
        // - DeletionTime (DateTime?)
        // - DeleterUserId (long?)
       // - IsDeleted (bool)
    }
}
