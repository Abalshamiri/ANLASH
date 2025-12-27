using System;
using System.ComponentModel.DataAnnotations;

namespace ANLASH.Storage.Dto
{
    /// <summary>
    /// Upload File Input DTO
    /// DTO لرفع الملفات
    /// </summary>
    public class UploadFileInput
    {
        /// <summary>
        /// File binary data | البيانات الثنائية للملف
        /// </summary>
        [Required]
        public byte[] FileBytes { get; set; }

        /// <summary>
        /// Original file name | اسم الملف الأصلي
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string FileName { get; set; }

        /// <summary>
        /// MIME content type | نوع المحتوى
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ContentType { get; set; }

        /// <summary>
        /// File description | وصف الملف
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// File category | فئة الملف
        /// </summary>
        [MaxLength(100)]
        public string Category { get; set; }

        /// <summary>
        /// Entity type this file belongs to | نوع الكيان المرتبط
        /// </summary>
        [MaxLength(100)]
        public string EntityType { get; set; }

        /// <summary>
        /// Entity ID this file belongs to | معرف الكيان المرتبط
        /// </summary>
        public long? EntityId { get; set; }
    }
}
