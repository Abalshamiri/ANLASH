using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using ANLASH.Authorization;
using ANLASH.LanguageCenters.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ANLASH.LanguageCenters
{
    /// <summary>
    /// خدمة التطبيق لتسعير الدورات - Course Pricing Application Service
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_CoursePricing)]
    public class CoursePricingAppService : AsyncCrudAppService<CoursePricing, CoursePricingDto, long, PagedCoursePricingRequestDto, CreateCoursePricingDto, UpdateCoursePricingDto>, ICoursePricingAppService
    {
        private readonly IRepository<LanguageCourse, long> _courseRepository;

        public CoursePricingAppService(
            IRepository<CoursePricing, long> repository,
            IRepository<LanguageCourse, long> courseRepository)
            : base(repository)
        {
            _courseRepository = courseRepository;
        }

        [AbpAuthorize(PermissionNames.Pages_CoursePricing_Create)]
        public override async Task<CoursePricingDto> CreateAsync(CreateCoursePricingDto input)
        {
            CheckCreatePermission();

            // Validate course exists
            var courseExists = await _courseRepository.GetAll()
                .AnyAsync(c => c.Id == input.LanguageCourseId);

            if (!courseExists)
                throw new UserFriendlyException("Language course not found");

            // Check for duplicate duration
            var duplicateExists = await Repository.GetAll()
                .AnyAsync(p => p.LanguageCourseId == input.LanguageCourseId && 
                              p.DurationWeeks == input.DurationWeeks);

            if (duplicateExists)
                throw new UserFriendlyException($"Pricing for {input.DurationWeeks} weeks already exists");

            var pricing = ObjectMapper.Map<CoursePricing>(input);
            
            // Calculate fee per week
            if (input.DurationWeeks > 0)
                pricing.FeePerWeek = input.Fee / input.DurationWeeks;

            // Calculate final price
            pricing.FinalPrice = CalculateFinalPrice(pricing);

            await Repository.InsertAsync(pricing);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<CoursePricingDto>(pricing);
        }

        [AbpAuthorize(PermissionNames.Pages_CoursePricing_Edit)]
        public override async Task<CoursePricingDto> UpdateAsync(UpdateCoursePricingDto input)
        {
            CheckUpdatePermission();

            var pricing = await Repository.GetAsync(input.Id);
            ObjectMapper.Map(input, pricing);

            // Recalculate fee per week
            if (pricing.DurationWeeks > 0)
                pricing.FeePerWeek = pricing.Fee / pricing.DurationWeeks;

            // Recalculate final price
            pricing.FinalPrice = CalculateFinalPrice(pricing);

            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<CoursePricingDto>(pricing);
        }

        /// <summary>
        /// جلب جميع الأسعار لدورة معينة
        /// </summary>
        public async Task<ListResultDto<CoursePricingDto>> GetByCourseAsync(long languageCourseId)
        {
            var pricing = await Repository.GetAll()
                .Where(p => p.LanguageCourseId == languageCourseId)
                .OrderBy(p => p.DurationWeeks)
                .ToListAsync();

            return new ListResultDto<CoursePricingDto>(
                ObjectMapper.Map<List<CoursePricingDto>>(pricing)
            );
        }

        /// <summary>
        /// جلب الأسعار النشطة لدورة
        /// </summary>
        public async Task<ListResultDto<CoursePricingDto>> GetActiveByCourseAsync(long languageCourseId)
        {
            var pricing = await Repository.GetAll()
                .Where(p => p.LanguageCourseId == languageCourseId && p.IsActive)
                .OrderBy(p => p.DurationWeeks)
                .ToListAsync();

            return new ListResultDto<CoursePricingDto>(
                ObjectMapper.Map<List<CoursePricingDto>>(pricing)
            );
        }

        /// <summary>
        /// إنشاء عدة أسعار دفعة واحدة
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_CoursePricing_Create)]
        public async Task CreateBulkAsync(List<CreateCoursePricingDto> input)
        {
            CheckCreatePermission();

            foreach (var dto in input)
            {
                await CreateAsync(dto);
            }
        }

        /// <summary>
        /// تحديث عدة أسعار دفعة واحدة
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_CoursePricing_Edit)]
        public async Task UpdateBulkAsync(List<UpdateCoursePricingDto> input)
        {
            CheckUpdatePermission();

            foreach (var dto in input)
            {
                await UpdateAsync(dto);
            }
        }

        /// <summary>
        /// حذف جميع الأسعار لدورة
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_CoursePricing_Delete)]
        public async Task DeleteByCourseAsync(long languageCourseId)
        {
            CheckDeletePermission();

            var pricing = await Repository.GetAll()
                .Where(p => p.LanguageCourseId == languageCourseId)
                .ToListAsync();

            foreach (var p in pricing)
            {
                await Repository.DeleteAsync(p);
            }
        }

        /// <summary>
        /// تطبيق خصم على جميع أسعار دورة
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_CoursePricing_Edit)]
        public async Task ApplyDiscountToCourseAsync(long languageCourseId, decimal discountPercentage)
        {
            CheckUpdatePermission();

            if (discountPercentage < 0 || discountPercentage > 100)
                throw new UserFriendlyException("Discount percentage must be between 0 and 100");

            var pricing = await Repository.GetAll()
                .Where(p => p.LanguageCourseId == languageCourseId)
                .ToListAsync();

            foreach (var p in pricing)
            {
                p.HasDiscount = discountPercentage > 0;
                p.DiscountPercentage = discountPercentage;
                p.DiscountAmount = p.Fee * (discountPercentage / 100);
                p.FinalPrice = CalculateFinalPrice(p);
            }

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// حساب السعر النهائي بعد الخصم
        /// </summary>
        public async Task<decimal> CalculateFinalPriceAsync(long pricingId)
        {
            var pricing = await Repository.GetAsync(pricingId);
            return CalculateFinalPrice(pricing);
        }

        #region Helper Methods

        protected override IQueryable<CoursePricing> CreateFilteredQuery(PagedCoursePricingRequestDto input)
        {
            var query = Repository.GetAll();

            if (input.LanguageCourseId.HasValue)
                query = query.Where(p => p.LanguageCourseId == input.LanguageCourseId.Value);

            if (input.DurationWeeks.HasValue)
                query = query.Where(p => p.DurationWeeks == input.DurationWeeks.Value);

            if (input.IsActive.HasValue)
                query = query.Where(p => p.IsActive == input.IsActive.Value);

            if (input.HasDiscount.HasValue)
                query = query.Where(p => p.HasDiscount == input.HasDiscount.Value);

            return query;
        }

        protected override IQueryable<CoursePricing> ApplySorting(IQueryable<CoursePricing> query, PagedCoursePricingRequestDto input)
        {
            if (!string.IsNullOrWhiteSpace(input.Sorting))
                return query.OrderBy(input.Sorting);

            return query.OrderBy(p => p.DurationWeeks);
        }

        private decimal CalculateFinalPrice(CoursePricing pricing)
        {
            if (!pricing.HasDiscount)
                return pricing.Fee;

            if (pricing.DiscountPercentage.HasValue && pricing.DiscountPercentage.Value > 0)
                return pricing.Fee * (1 - pricing.DiscountPercentage.Value / 100);

            if (pricing.DiscountAmount.HasValue && pricing.DiscountAmount.Value > 0)
                return pricing.Fee - pricing.DiscountAmount.Value;

            return pricing.Fee;
        }

        #endregion
    }
}
