using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ANLASH.Authorization;
using ANLASH.LanguageCenters.Dto;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ANLASH.LanguageCenters
{
    /// <summary>
    /// خدمة التطبيق للأسئلة الشائعة للمعاهد - Language Center FAQ Application Service
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_LanguageCenterFAQs)]
    public class LanguageCenterFAQAppService : AsyncCrudAppService<LanguageCenterFAQ, LanguageCenterFAQDto, long, PagedLanguageCenterFAQRequestDto, CreateLanguageCenterFAQDto, UpdateLanguageCenterFAQDto>, ILanguageCenterFAQAppService
    {
        private readonly IRepository<LanguageCenter, long> _languageCenterRepository;

        public LanguageCenterFAQAppService(
            IRepository<LanguageCenterFAQ, long> repository,
            IRepository<LanguageCenter, long> languageCenterRepository)
            : base(repository)
        {
            _languageCenterRepository = languageCenterRepository;
        }

        [AbpAuthorize(PermissionNames.Pages_LanguageCenterFAQs_Create)]
        public override async Task<LanguageCenterFAQDto> CreateAsync(CreateLanguageCenterFAQDto input)
        {
            CheckCreatePermission();

            // Validate language center exists
            var centerExists = await _languageCenterRepository.GetAll()
                .AnyAsync(lc => lc.Id == input.LanguageCenterId);

            if (!centerExists)
                throw new UserFriendlyException("Language center not found");

            var faq = ObjectMapper.Map<LanguageCenterFAQ>(input);
            await Repository.InsertAsync(faq);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<LanguageCenterFAQDto>(faq);
        }

        [AbpAuthorize(PermissionNames.Pages_LanguageCenterFAQs_Edit)]
        public override async Task<LanguageCenterFAQDto> UpdateAsync(UpdateLanguageCenterFAQDto input)
        {
            CheckUpdatePermission();

            var faq = await Repository.GetAsync(input.Id);
            ObjectMapper.Map(input, faq);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<LanguageCenterFAQDto>(faq);
        }

        [AbpAuthorize(PermissionNames.Pages_LanguageCenterFAQs_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            CheckDeletePermission();
            await Repository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// جلب جميع الأسئلة لمعهد معين
        /// </summary>
        public async Task<ListResultDto<LanguageCenterFAQDto>> GetByLanguageCenterAsync(long languageCenterId)
        {
            var faqs = await Repository.GetAll()
                .Where(f => f.LanguageCenterId == languageCenterId)
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();

            return new ListResultDto<LanguageCenterFAQDto>(
                ObjectMapper.Map<List<LanguageCenterFAQDto>>(faqs)
            );
        }

        /// <summary>
        /// جلب الأسئلة المنشورة لمعهد
        /// </summary>
        public async Task<ListResultDto<LanguageCenterFAQDto>> GetPublishedByLanguageCenterAsync(long languageCenterId)
        {
            var faqs = await Repository.GetAll()
                .Where(f => f.LanguageCenterId == languageCenterId && f.IsPublished)
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();

            return new ListResultDto<LanguageCenterFAQDto>(
                ObjectMapper.Map<List<LanguageCenterFAQDto>>(faqs)
            );
        }

        /// <summary>
        /// جلب الأسئلة حسب الفئة
        /// </summary>
        public async Task<ListResultDto<LanguageCenterFAQDto>> GetByCategoryAsync(long languageCenterId, string category)
        {
            var faqs = await Repository.GetAll()
                .Where(f => f.LanguageCenterId == languageCenterId && 
                           f.IsPublished &&
                           (f.Category == category || f.CategoryAr == category))
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();

            return new ListResultDto<LanguageCenterFAQDto>(
                ObjectMapper.Map<List<LanguageCenterFAQDto>>(faqs)
            );
        }

        /// <summary>
        /// جلب الأسئلة المميزة
        /// </summary>
        public async Task<ListResultDto<LanguageCenterFAQDto>> GetFeaturedAsync(long languageCenterId)
        {
            var faqs = await Repository.GetAll()
                .Where(f => f.LanguageCenterId == languageCenterId && 
                           f.IsPublished && 
                           f.IsFeatured)
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();

            return new ListResultDto<LanguageCenterFAQDto>(
                ObjectMapper.Map<List<LanguageCenterFAQDto>>(faqs)
            );
        }

        /// <summary>
        /// نشر/إلغاء نشر سؤال
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_LanguageCenterFAQs_Edit)]
        public async Task TogglePublishAsync(long id)
        {
            var faq = await Repository.GetAsync(id);
            faq.IsPublished = !faq.IsPublished;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// جعل سؤال مميز
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_LanguageCenterFAQs_Edit)]
        public async Task ToggleFeaturedAsync(long id)
        {
            var faq = await Repository.GetAsync(id);
            faq.IsFeatured = !faq.IsFeatured;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// تسجيل مشاهدة سؤال
        /// </summary>
        public async Task RecordViewAsync(long id)
        {
            var faq = await Repository.GetAsync(id);
            faq.ViewCount++;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// تسجيل تقييم سؤال (مفيد/غير مفيد)
        /// </summary>
        public async Task RecordRatingAsync(long id, bool isHelpful)
        {
            var faq = await Repository.GetAsync(id);
            
            if (isHelpful)
                faq.HelpfulCount++;
            else
                faq.NotHelpfulCount++;

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// إعادة ترتيب الأسئلة
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_LanguageCenterFAQs_Edit)]
        public async Task ReorderAsync(FAQOrderDto[] orders)
        {
            CheckUpdatePermission();

            foreach (var order in orders)
            {
                var faq = await Repository.GetAsync(order.Id);
                faq.DisplayOrder = order.DisplayOrder;
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        #region Helper Methods

        protected override IQueryable<LanguageCenterFAQ> CreateFilteredQuery(PagedLanguageCenterFAQRequestDto input)
        {
            var query = Repository.GetAll();

            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(f =>
                    f.Question.Contains(input.Keyword) ||
                    f.QuestionAr.Contains(input.Keyword) ||
                    f.Answer.Contains(input.Keyword) ||
                    f.AnswerAr.Contains(input.Keyword));
            }

            if (input.LanguageCenterId.HasValue)
                query = query.Where(f => f.LanguageCenterId == input.LanguageCenterId.Value);

            if (!string.IsNullOrWhiteSpace(input.Category))
                query = query.Where(f => f.Category == input.Category || f.CategoryAr == input.Category);

            if (input.IsPublished.HasValue)
                query = query.Where(f => f.IsPublished == input.IsPublished.Value);

            if (input.IsFeatured.HasValue)
                query = query.Where(f => f.IsFeatured == input.IsFeatured.Value);

            return query;
        }

        protected override IQueryable<LanguageCenterFAQ> ApplySorting(IQueryable<LanguageCenterFAQ> query, PagedLanguageCenterFAQRequestDto input)
        {
            if (!string.IsNullOrWhiteSpace(input.Sorting))
                return query.OrderBy(input.Sorting);

            return query.OrderBy(f => f.DisplayOrder);
        }

        #endregion
    }
}
