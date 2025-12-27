using System.ComponentModel.DataAnnotations;
using ANLASH.Universities;

namespace ANLASH.Universities.Dto
{
    /// <summary>
    /// Create University Content DTO
    /// DTO لإنشاء محتوى جامعة جديد
    /// </summary>
    public class CreateUniversityContentDto
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
        public bool IsActive { get; set; } = true;
    }
}
