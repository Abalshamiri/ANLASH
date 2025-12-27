namespace ANLASH.Storage.Dto
{
    /// <summary>
    /// Download File Output DTO
    /// DTO لتنزيل الملفات
    /// </summary>
    public class DownloadFileOutput
    {
        /// <summary>
        /// File binary data | البيانات الثنائية للملف
        /// </summary>
        public byte[] FileBytes { get; set; }

        /// <summary>
        /// Original file name | اسم الملف الأصلي
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// MIME content type | نوع المحتوى
        /// </summary>
        public string ContentType { get; set; }
    }
}
