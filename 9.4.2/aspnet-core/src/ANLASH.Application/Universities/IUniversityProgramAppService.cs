using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ANLASH.Universities.Dto;

namespace ANLASH.Universities
{
    /// <summary>
    /// University Program Application Service Interface
    /// واجهة خدمة تطبيق برامج الجامعة
    /// </summary>
    public interface IUniversityProgramAppService : IAsyncCrudAppService<
        UniversityProgramDto,
        long,
        PagedAndSortedResultRequestDto,
        CreateUniversityProgramDto,
        UpdateUniversityProgramDto>
    {
        /// <summary>
        /// Get all programs for a specific university
        /// الحصول على جميع برامج جامعة معينة
        /// </summary>
        Task<ListResultDto<UniversityProgramDto>> GetByUniversityIdAsync(long universityId);
        
        /// <summary>
        /// Get programs by level
        /// الحصول على البرامج حسب المستوى
        /// </summary>
        Task<PagedResultDto<UniversityProgramDto>> GetByLevelAsync(ProgramLevel level, PagedAndSortedResultRequestDto input);
        
        /// <summary>
        /// Get featured programs
        /// الحصول على البرامج المميزة
        /// </summary>
        Task<ListResultDto<UniversityProgramDto>> GetFeaturedAsync(int maxCount = 10);
        
        /// <summary>
        /// Toggle featured status
        /// تبديل حالة التمييز
        /// </summary>
        Task ToggleFeaturedAsync(long id);
    }
}
