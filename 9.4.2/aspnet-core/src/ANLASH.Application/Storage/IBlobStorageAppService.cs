using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using ANLASH.Storage.Dto;

namespace ANLASH.Storage
{
    /// <summary>
    /// Blob Storage Application Service Interface
    /// واجهة خدمة تطبيق تخزين الملفات
    /// </summary>
    public interface IBlobStorageAppService : IApplicationService
    {
        /// <summary>
        /// Upload file
        /// رفع ملف
        /// </summary>
        Task<FileDto> UploadAsync(UploadFileInput input);

        /// <summary>
        /// Download file
        /// تنزيل ملف
        /// </summary>
        Task<DownloadFileOutput> DownloadAsync(Guid id);

        /// <summary>
        /// Get file info
        /// الحصول على معلومات الملف
        /// </summary>
        Task<FileDto> GetFileInfoAsync(Guid id);

        /// <summary>
        /// Delete file
        /// حذف ملف
        /// </summary>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Get files by entity
        /// الحصول على ملفات كيان معين
        /// </summary>
        Task<List<FileDto>> GetFilesByEntityAsync(string entityType, long entityId);

        /// <summary>
        /// Get files by category
        /// الحصول على ملفات حسب الفئة
        /// </summary>
        Task<List<FileDto>> GetFilesByCategoryAsync(string category);
    }
}
