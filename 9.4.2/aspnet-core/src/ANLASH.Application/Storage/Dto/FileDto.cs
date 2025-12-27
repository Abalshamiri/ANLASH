using System;
using Abp.Application.Services.Dto;

namespace ANLASH.Storage.Dto
{
    /// <summary>
    /// File DTO
    /// DTO لمعلومات الملف
    /// </summary>
    public class FileDto : EntityDto<Guid>
    {
        /// <summary>
        /// Original file name | اسم الملف الأصلي
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// MIME content type | نوع المحتوى
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// File size in bytes | حجم الملف بالبايت
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// File description | وصف الملف
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// File category | فئة الملف
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Entity type | نوع الكيان المرتبط
        /// </summary>
        public string EntityType { get; set; }

        /// <summary>
        /// Entity ID | معرف الكيان المرتبط
        /// </summary>
        public long? EntityId { get; set; }

        /// <summary>
        /// Image width in pixels | عرض الصورة بالبكسل
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Image height in pixels | ارتفاع الصورة بالبكسل
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// Creation time | تاريخ الإنشاء
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Creator user ID | معرف المستخدم المنشئ
        /// </summary>
        public long? CreatorUserId { get; set; }
    }
}
