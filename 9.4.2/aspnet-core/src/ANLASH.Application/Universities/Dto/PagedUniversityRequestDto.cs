using Abp.Application.Services.Dto;

namespace ANLASH.Universities.Dto
{
    /// <summary>
    /// DTO للبحث والصفحات - Paged Request DTO
    /// </summary>
    public class PagedUniversityRequestDto : PagedResultRequestDto
    {
        /// <summary>
        /// كلمة البحث - Search term
        /// </summary>
        public string SearchTerm { get; set; }

        /// <summary>
        /// الدولة - Country filter
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// المدينة - City filter
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// نوع الجامعة - University Type filter
        /// </summary>
        public UniversityType? Type { get; set; }

        /// <summary>
        /// جامعات مميزة فقط - Featured only
        /// </summary>
        public bool? IsFeatured { get; set; }

        /// <summary>
        /// جامعات نشطة فقط - Active only
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// الحد الأدنى للتقييم - Minimum rating
        /// </summary>
        public decimal? MinRating { get; set; }

        /// <summary>
        /// ترتيب حسب - Order by field
        /// </summary>
        public string OrderBy { get; set; } = "Name";

        /// <summary>
        /// تنازلي - Descending order
        /// </summary>
        public bool IsDescending { get; set; }
    }
}
