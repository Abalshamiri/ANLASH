using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace ANLASH.Storage
{
    /// <summary>
    /// App Binary Object Entity for storing files
    /// كيان لتخزين الملفات في قاعدة البيانات
    /// </summary>
    [Table("AppBinaryObjects")]
    public class AppBinaryObject : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        /// <summary>
        /// Tenant ID for multi-tenancy support
        /// معرف المستأجر لدعم تعدد المستأجرين
        /// </summary>
        public int? TenantId { get; set; }

        #region File Info | معلومات الملف

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
        /// File size in bytes | حجم الملف بالبايت
        /// </summary>
        public long FileSize { get; set; }

        #endregion

        #region Storage | التخزين

        /// <summary>
        /// File binary data | البيانات الثنائية للملف
        /// </summary>
        [Required]
        public byte[] Bytes { get; set; }

        #endregion

        #region Metadata | البيانات الوصفية

        /// <summary>
        /// File description | وصف الملف
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        /// <summary>
        /// File category (e.g., UniversityLogo, CoverImage) | فئة الملف
        /// </summary>
        [MaxLength(100)]
        public string Category { get; set; }

        #endregion

        #region Reference | المرجع

        /// <summary>
        /// Entity type this file belongs to | نوع الكيان المرتبط
        /// </summary>
        [MaxLength(100)]
        public string EntityType { get; set; }

        /// <summary>
        /// Entity ID this file belongs to | معرف الكيان المرتبط
        /// </summary>
        public long? EntityId { get; set; }

        #endregion

        #region Image Metadata | معلومات الصورة

        /// <summary>
        /// Image width in pixels (if image) | عرض الصورة بالبكسل
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Image height in pixels (if image) | ارتفاع الصورة بالبكسل
        /// </summary>
        public int? Height { get; set; }

        #endregion

        // Note: Audit fields inherited from FullAuditedEntity<Guid>:
        // - CreationTime, CreatorUserId
        // - LastModificationTime, LastModifierUserId
        // - DeletionTime, DeleterUserId, IsDeleted
    }
}
