using Abp.Application.Services;
using Abp.Application.Services.Dto;
using ANLASH.LanguageCenters.Dto;
using System.Threading.Tasks;

namespace ANLASH.LanguageCenters
{
    /// <summary>
    /// خدمة التطبيق للأسئلة الشائعة للمعاهد - Language Center FAQ Application Service Interface
    /// </summary>
    public interface ILanguageCenterFAQAppService : IAsyncCrudAppService<LanguageCenterFAQDto, long, PagedLanguageCenterFAQRequestDto, CreateLanguageCenterFAQDto, UpdateLanguageCenterFAQDto>
    {
        /// <summary>
        /// جلب جميع الأسئلة لمعهد معين - Get all FAQs for a specific language center
        /// </summary>
        Task<ListResultDto<LanguageCenterFAQDto>> GetByLanguageCenterAsync(long languageCenterId);

        /// <summary>
        /// جلب الأسئلة المنشورة لمعهد - Get published FAQs for a language center
        /// </summary>
        Task<ListResultDto<LanguageCenterFAQDto>> GetPublishedByLanguageCenterAsync(long languageCenterId);

        /// <summary>
        /// جلب الأسئلة حسب الفئة - Get FAQs by category
        /// </summary>
        Task<ListResultDto<LanguageCenterFAQDto>> GetByCategoryAsync(long languageCenterId, string category);

        /// <summary>
        /// جلب الأسئلة المميزة - Get featured FAQs
        /// </summary>
        Task<ListResultDto<LanguageCenterFAQDto>> GetFeaturedAsync(long languageCenterId);

        /// <summary>
        /// نشر/إلغاء نشر سؤال - Publish/Unpublish FAQ
        /// </summary>
        Task TogglePublishAsync(long id);

        /// <summary>
        /// جعل سؤال مميز - Toggle featured status
        /// </summary>
        Task ToggleFeaturedAsync(long id);

        /// <summary>
        /// تسجيل مشاهدة سؤال - Record FAQ view
        /// </summary>
        Task RecordViewAsync(long id);

        /// <summary>
        /// تسجيل تقييم سؤال (مفيد/غير مفيد) - Record FAQ rating
        /// </summary>
        Task RecordRatingAsync(long id, bool isHelpful);

        /// <summary>
        /// إعادة ترتيب الأسئلة - Reorder FAQs
        /// </summary>
        Task ReorderAsync(FAQOrderDto[] orders);
    }
}
