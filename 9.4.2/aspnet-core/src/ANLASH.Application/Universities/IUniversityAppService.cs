using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ANLASH.Universities.Dto;
using System.Threading.Tasks;

namespace ANLASH.Universities
{
    /// <summary>
    /// خدمة التطبيق للجامعات - University Application Service Interface
    /// </summary>
    public interface IUniversityAppService : IAsyncCrudAppService<UniversityDto, long, PagedUniversityRequestDto, CreateUniversityDto, UpdateUniversityDto>
    {
        /// <summary>
        /// جلب جميع الجامعات النشطة - Get all active universities
        /// </summary>
        Task<PagedResultDto<UniversityDto>> GetAllActiveAsync(PagedUniversityRequestDto input);

        /// <summary>
        /// جلب الجامعات المميزة - Get featured universities
        /// </summary>
        Task<ListResultDto<UniversityDto>> GetFeaturedAsync(int count = 10);

        /// <summary>
        /// جلب جامعة حسب Slug - Get university by slug
        /// </summary>
        Task<UniversityDto> GetBySlugAsync(string slug);

        /// <summary>
        /// Get complete university details with all related data (Programs, FAQs, Contents)
        /// الحصول على تفاصيل الجامعة الكاملة مع جميع البيانات المرتبطة
        /// </summary>
        Task<UniversityDetailDto> GetUniversityDetailAsync(long id);

        /// <summary>
        /// Get complete university details by slug (for public pages)
        /// الحصول على تفاصيل الجامعة بواسطة Slug (للصفحات العامة)
        /// </summary>
        Task<UniversityDetailDto> GetUniversityDetailBySlugAsync(string slug);

        /// <summary>
        /// تفعيل/إلغاء تفعيل جامعة - Activate/Deactivate university
        /// </summary>
        Task ToggleActiveAsync(long id);

        /// <summary>
        /// جعل جامعة مميزة - Make university featured
        /// </summary>
        Task ToggleFeaturedAsync(long id);
    }
}
