using Abp.Dependency;
using Abp.Domain.Services;
using Abp.UI;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ANLASH.Storage
{
    /// <summary>
    /// تطبيق خدمة التخزين المحلي للملفات
    /// <para>Local file storage service implementation</para>
    /// </summary>
    public class LocalFileStorageService : DomainService, IFileStorageService, ITransientDependency
    {
        private readonly IHostEnvironment _hostEnvironment;
        private const string UploadsFolder = "uploads";

        public LocalFileStorageService(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// حفظ ملف في التخزين المحلي
        /// </summary>
        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string folder)
        {
            try
            {
                // التحقق من صحة المدخلات - Validate inputs
                if (fileStream == null || fileStream.Length == 0)
                {
                    throw new UserFriendlyException(L("FileStorage:EmptyFile"));
                }

                if (string.IsNullOrWhiteSpace(fileName))
                {
                    throw new UserFriendlyException(L("FileStorage:InvalidFileName"));
                }

                // حماية من Path Traversal - Path traversal protection
                fileName = Path.GetFileName(fileName);
                folder = folder?.Replace("..", "").Replace("/", "\\") ?? "general";

                // إنشاء المسار الكامل - Build full path
                var uploadsPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", UploadsFolder, folder);
                
                // إنشاء المجلد إن لم يكن موجوداً - Create directory if not exists
                if (!Directory.Exists(uploadsPath))
                {
                    Directory.CreateDirectory(uploadsPath);
                }

                // إنشاء اسم ملف فريد - Generate unique filename
                var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                var filePath = Path.Combine(uploadsPath, uniqueFileName);

                // حفظ الملف - Save file
                using (var fileStreamToWrite = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(fileStreamToWrite);
                }

                // إرجاع المسار النسبي - Return relative path
                return Path.Combine(UploadsFolder, folder, uniqueFileName);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error("Error saving file", ex);
                throw new UserFriendlyException(L("FileStorage:SaveError"));
            }
        }

        /// <summary>
        /// جلب ملف من التخزين
        /// </summary>
        public async Task<Stream> GetFileAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new UserFriendlyException(L("FileStorage:InvalidFilePath"));
                }

                // بناء المسار الكامل - Build full path
                var fullPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", filePath);

                // التحقق من وجود الملف - Check file exists
                if (!File.Exists(fullPath))
                {
                    throw new UserFriendlyException(L("FileStorage:FileNotFound"));
                }

                // قراءة الملف - Read file
                var memoryStream = new MemoryStream();
                using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
                {
                    await fileStream.CopyToAsync(memoryStream);
                }
                memoryStream.Position = 0;
                
                return memoryStream;
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error("Error getting file", ex);
                throw new UserFriendlyException(L("FileStorage:GetError"));
            }
        }

        /// <summary>
        /// حذف ملف من التخزين
        /// </summary>
        public async Task DeleteFileAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    throw new UserFriendlyException(L("FileStorage:InvalidFilePath"));
                }

                // بناء المسار الكامل - Build full path
                var fullPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", filePath);

                // التحقق من وجود الملف - Check file exists
                if (!File.Exists(fullPath))
                {
                    throw new UserFriendlyException(L("FileStorage:FileNotFound"));
                }

                // حذف الملف - Delete file
                File.Delete(fullPath);
                
                await Task.CompletedTask;
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error("Error deleting file", ex);
                throw new UserFriendlyException(L("FileStorage:DeleteError"));
            }
        }

        /// <summary>
        /// التحقق من وجود ملف
        /// </summary>
        public async Task<bool> FileExistsAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    return false;
                }

                var fullPath = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot", filePath);
                return await Task.FromResult(File.Exists(fullPath));
            }
            catch (Exception ex)
            {
                Logger.Error("Error checking file existence", ex);
                return false;
            }
        }
    }
}
