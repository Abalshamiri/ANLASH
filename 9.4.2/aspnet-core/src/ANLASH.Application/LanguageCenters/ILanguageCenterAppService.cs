using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ANLASH.LanguageCenters.Dto;
using System.Threading.Tasks;

namespace ANLASH.LanguageCenters
{
    /// <summary>
    /// خدمة التطبيق لمعاهد اللغة - Language Center Application Service Interface
    /// <para>Provides CRUD operations and business logic for language centers</para>
    /// </summary>
    public interface ILanguageCenterAppService : IAsyncCrudAppService<LanguageCenterDto, long, PagedLanguageCenterRequestDto, CreateLanguageCenterDto, UpdateLanguageCenterDto>
    {
        /// <summary>
        /// جلب جميع المعاهد النشطة - Get all active language centers
        /// </summary>
        Task<PagedResultDto<LanguageCenterDto>> GetAllActiveAsync(PagedLanguageCenterRequestDto input);

        /// <summary>
        /// جلب المعاهد المميزة - Get featured language centers
        /// </summary>
        Task<ListResultDto<LanguageCenterDto>> GetFeaturedAsync(int count = 10);

        /// <summary>
        /// جلب معهد حسب Slug - Get language center by slug
        /// </summary>
        Task<LanguageCenterDto> GetBySlugAsync(string slug);

        /// <summary>
        /// Get complete language center details with all related data (Courses, Pricing, FAQs)
        /// الحصول على تفاصيل المعهد الكاملة مع جميع البيانات المرتبطة
        /// </summary>
        Task<LanguageCenterDetailDto> GetLanguageCenterDetailAsync(long id);

        /// <summary>
        /// Get complete language center details by slug (for public pages)
        /// الحصول على تفاصيل المعهد بواسطة Slug (للصفحات العامة)
        /// </summary>
        Task<LanguageCenterDetailDto> GetLanguageCenterDetailBySlugAsync(string slug);

        /// <summary>
        /// جلب المعاهد حسب الدولة - Get language centers by country
        /// </summary>
        Task<ListResultDto<LanguageCenterDto>> GetByCountryAsync(int countryId);

        /// <summary>
        /// جلب المعاهد المعتمدة - Get accredited language centers
        /// </summary>
        Task<PagedResultDto<LanguageCenterDto>> GetAccreditedAsync(PagedLanguageCenterRequestDto input);

        /// <summary>
        /// تفعيل/إلغاء تفعيل معهد - Activate/Deactivate language center
        /// </summary>
        Task ToggleActiveAsync(long id);

        /// <summary>
        /// جعل معهد مميز - Make language center featured
        /// </summary>
        Task ToggleFeaturedAsync(long id);
    }
}
