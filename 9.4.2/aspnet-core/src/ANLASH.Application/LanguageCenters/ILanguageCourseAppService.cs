using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ANLASH.LanguageCenters.Dto;
using System.Threading.Tasks;

namespace ANLASH.LanguageCenters
{
    /// <summary>
    /// خدمة التطبيق للدورات اللغوية - Language Course Application Service Interface
    /// </summary>
    public interface ILanguageCourseAppService : IAsyncCrudAppService<LanguageCourseDto, long, PagedLanguageCourseRequestDto, CreateLanguageCourseDto, UpdateLanguageCourseDto>
    {
        /// <summary>
        /// جلب جميع الدورات لمعهد معين - Get all courses for a specific language center
        /// </summary>
        Task<ListResultDto<LanguageCourseDto>> GetByLanguageCenterAsync(long languageCenterId);

        /// <summary>
        /// جلب الدورات النشطة لمعهد - Get active courses for a language center
        /// </summary>
        Task<ListResultDto<LanguageCourseDto>> GetActiveByLanguageCenterAsync(long languageCenterId);

        /// <summary>
        /// جلب الدورات حسب النوع - Get courses by type
        /// </summary>
        Task<PagedResultDto<LanguageCourseDto>> GetByCourseTypeAsync(CourseType courseType, PagedLanguageCourseRequestDto input);

        /// <summary>
        /// جلب الدورات حسب المستوى - Get courses by level
        /// </summary>
        Task<PagedResultDto<LanguageCourseDto>> GetByLevelAsync(CourseLevel level, PagedLanguageCourseRequestDto input);

        /// <summary>
        /// جلب الدورات المميزة - Get featured courses
        /// </summary>
        Task<ListResultDto<LanguageCourseDto>> GetFeaturedAsync(int count = 10);

        /// <summary>
        /// تفعيل/إلغاء تفعيل دورة - Toggle course active status
        /// </summary>
        Task ToggleActiveAsync(long id);
    }
}
