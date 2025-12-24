using System.IO;
using System.Threading.Tasks;

namespace ANLASH.Storage
{
    /// <summary>
    /// خدمة إدارة الملفات - File Storage Service
    /// <para>Provides functionality for managing file storage operations</para>
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// حفظ ملف في التخزين المحلي
        /// <para>Save a file to local storage</para>
        /// </summary>
        /// <param name="fileStream">البيانات الثنائية للملف - File binary data</param>
        /// <param name="fileName">اسم الملف - File name</param>
        /// <param name="folder">مجلد الحفظ - Destination folder</param>
        /// <returns>المسار الكامل للملف المحفوظ - Full path of saved file</returns>
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string folder);

        /// <summary>
        /// جلب ملف من التخزين
        /// <para>Get a file from storage</para>
        /// </summary>
        /// <param name="filePath">المسار الكامل للملف - Full file path</param>
        /// <returns>البيانات الثنائية للملف - File binary data</returns>
        Task<Stream> GetFileAsync(string filePath);

        /// <summary>
        /// حذف ملف من التخزين
        /// <para>Delete a file from storage</para>
        /// </summary>
        /// <param name="filePath">المسار الكامل للملف - Full file path</param>
        Task DeleteFileAsync(string filePath);

        /// <summary>
        /// التحقق من وجود ملف
        /// <para>Check if a file exists</para>
        /// </summary>
        /// <param name="filePath">المسار الكامل للملف - Full file path</param>
        /// <returns>true إذا كان الملف موجوداً - true if file exists</returns>
        Task<bool> FileExistsAsync(string filePath);
    }
}
