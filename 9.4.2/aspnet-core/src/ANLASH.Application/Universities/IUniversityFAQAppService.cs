using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ANLASH.Universities.Dto;

namespace ANLASH.Universities
{
    /// <summary>
    /// University FAQ Application Service Interface
    /// واجهة خدمة الأسئلة الشائعة للجامعة
    /// </summary>
    public interface IUniversityFAQAppService : IApplicationService
    {
        /// <summary>
        /// Get FAQ by ID
        /// الحصول على سؤال بواسطة المعرف
        /// </summary>
        Task<UniversityFAQDto> GetAsync(long id);

        /// <summary>
        /// Get all FAQs for a specific university
        /// الحصول على جميع الأسئلة الشائعة لجامعة معينة
        /// </summary>
        Task<ListResultDto<UniversityFAQDto>> GetByUniversityAsync(long universityId);

        /// <summary>
        /// Get published FAQs for a specific university (public)
        /// الحصول على الأسئلة المنشورة لجامعة معينة (عامة)
        /// </summary>
        Task<ListResultDto<UniversityFAQDto>> GetPublishedByUniversityAsync(long universityId);

        /// <summary>
        /// Create a new FAQ
        /// إنشاء سؤال جديد
        /// </summary>
        Task<UniversityFAQDto> CreateAsync(CreateUniversityFAQDto input);

        /// <summary>
        /// Update an existing FAQ
        /// تحديث سؤال موجود
        /// </summary>
        Task<UniversityFAQDto> UpdateAsync(UpdateUniversityFAQDto input);

        /// <summary>
        /// Delete a FAQ
        /// حذف سؤال
        /// </summary>
        Task DeleteAsync(long id);

        /// <summary>
        /// Reorder FAQs
        /// إعادة ترتيب الأسئلة
        /// </summary>
        Task ReorderAsync(List<FAQOrderDto> input);

        /// <summary>
        /// Toggle publish status
        /// تبديل حالة النشر
        /// </summary>
        Task<UniversityFAQDto> TogglePublishAsync(long id);
    }
}
