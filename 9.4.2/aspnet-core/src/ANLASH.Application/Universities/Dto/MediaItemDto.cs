using System;
using Abp.Application.Services.Dto;

namespace ANLASH.Universities.Dto
{
    /// <summary>
    /// DTO for gallery images and media items
    /// DTO لصور المعرض والوسائط
    /// </summary>
    public class MediaItemDto : EntityDto<Guid>
    {
        /// <summary>
        /// Media URL | رابط الوسيط
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Thumbnail URL | رابط الصورة المصغرة
        /// </summary>
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// Title (English) | العنوان بالإنجليزية
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Title (Arabic) | العنوان بالعربية
        /// </summary>
        public string TitleAr { get; set; }

        /// <summary>
        /// Description (English) | الوصف بالإنجليزية
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Description (Arabic) | الوصف بالعربية
        /// </summary>
        public string DescriptionAr { get; set; }

        /// <summary>
        /// Display order | ترتيب العرض
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Media type (Image, Video) | نوع الوسيط
        /// </summary>
        public string MediaType { get; set; }
    }
}
