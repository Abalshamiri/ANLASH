using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ANLASH.Universities.Dto;

namespace ANLASH.Universities
{
    /// <summary>
    /// University Content Application Service Interface
    /// واجهة خدمة تطبيق محتوى الجامعة
    /// </summary>
    public interface IUniversityContentAppService : IAsyncCrudAppService<
        UniversityContentDto,
        long,
        PagedAndSortedResultRequestDto,
        CreateUniversityContentDto,
        UpdateUniversityContentDto>
    {
        /// <summary>
        /// Get all contents for a specific university
        /// الحصول على جميع محتويات جامعة معينة
        /// </summary>
        /// <param name="universityId">University ID | معرف الجامعة</param>
        /// <returns>List of contents | قائمة المحتويات</returns>
        Task<ListResultDto<UniversityContentDto>> GetByUniversityIdAsync(long universityId);
        
        /// <summary>
        /// Get content by type for a university
        /// الحصول على محتوى حسب النوع لجامعة معينة
        /// </summary>
        /// <param name="universityId">University ID | معرف الجامعة</param>
        /// <param name="contentType">Content type | نوع المحتوى</param>
        /// <returns>Content DTO | DTO المحتوى</returns>
        Task<UniversityContentDto> GetByTypeAsync(long universityId, UniversityContentType contentType);
        
        /// <summary>
        /// Reorder contents for a university
        /// إعادة ترتيب محتويات جامعة
        /// </summary>
        /// <param name="universityId">University ID | معرف الجامعة</param>
        /// <param name="contentIds">Ordered list of content IDs | قائمة معرفات المحتوى المرتبة</param>
        Task ReorderAsync(long universityId, List<long> contentIds);
    }
}
