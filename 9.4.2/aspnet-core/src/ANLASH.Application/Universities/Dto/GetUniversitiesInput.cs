using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;

namespace ANLASH.Universities.Dto
{
    /// <summary>
    /// Input DTO for getting universities list with filters
    /// DTO لإدخال معايير البحث والفلترة
    /// </summary>
    public class GetUniversitiesInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        /// <summary>
        /// Search filter (name, city, country) | فلتر البحث
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// Filter by country ID | الفلترة حسب الدولة
        /// </summary>
        public int? CountryId { get; set; }

        /// <summary>
        /// Filter by city ID | الفلترة حسب المدينة
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// Filter by featured status | الفلترة حسب الحالة المميزة
        /// </summary>
        public bool? IsFeatured { get; set; }

        /// <summary>
        /// Filter by active status | الفلترة حسب الحالة النشطة
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Filter by university type | الفلترة حسب نوع الجامعة
        /// </summary>
        public UniversityType? Type { get; set; }

        /// <summary>
        /// Normalize sorting (default: Featured DESC, Name ASC)
        /// </summary>
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "IsFeatured DESC, Name";
            }
        }
    }
}
