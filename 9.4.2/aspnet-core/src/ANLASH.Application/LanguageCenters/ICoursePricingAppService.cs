using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ANLASH.LanguageCenters.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ANLASH.LanguageCenters
{
    /// <summary>
    /// خدمة التطبيق لتسعير الدورات - Course Pricing Application Service Interface
    /// </summary>
    public interface ICoursePricingAppService : IAsyncCrudAppService<CoursePricingDto, long, PagedCoursePricingRequestDto, CreateCoursePricingDto, UpdateCoursePricingDto>
    {
        /// <summary>
        /// جلب جميع الأسعار لدورة معينة - Get all pricing for a specific course
        /// </summary>
        Task<ListResultDto<CoursePricingDto>> GetByCourseAsync(long languageCourseId);

        /// <summary>
        /// جلب الأسعار النشطة لدورة - Get active pricing for a course
        /// </summary>
        Task<ListResultDto<CoursePricingDto>> GetActiveByCourseAsync(long languageCourseId);

        /// <summary>
        /// إنشاء عدة أسعار دفعة واحدة - Create multiple pricing entries at once
        /// <para>Useful for creating pricing table (1, 2, 4, 8, 12 weeks, etc.)</para>
        /// </summary>
        Task CreateBulkAsync(List<CreateCoursePricingDto> input);

        /// <summary>
        /// تحديث عدة أسعار دفعة واحدة - Update multiple pricing entries at once
        /// </summary>
        Task UpdateBulkAsync(List<UpdateCoursePricingDto> input);

        /// <summary>
        /// حذف جميع الأسعار لدورة - Delete all pricing for a course
        /// </summary>
        Task DeleteByCourseAsync(long languageCourseId);

        /// <summary>
        /// تطبيق خصم على جميع أسعار دورة - Apply discount to all pricing for a course
        /// </summary>
        Task ApplyDiscountToCourseAsync(long languageCourseId, decimal discountPercentage);

        /// <summary>
        /// حساب السعر النهائي بعد الخصم - Calculate final price after discount
        /// </summary>
        Task<decimal> CalculateFinalPriceAsync(long pricingId);
    }
}
