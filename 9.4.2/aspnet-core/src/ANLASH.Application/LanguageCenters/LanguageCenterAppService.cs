using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Caching;
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
    /// خدمة التطبيق لمعاهد اللغة - Language Center Application Service
    /// <para>Handles all language center operations with bilingual support, caching, and validation</para>
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_LanguageCenters)]
    public class LanguageCenterAppService : AsyncCrudAppService<LanguageCenter, LanguageCenterDto, long, PagedLanguageCenterRequestDto, CreateLanguageCenterDto, UpdateLanguageCenterDto>, ILanguageCenterAppService
    {
        private readonly ICacheManager _cacheManager;
        private const string LanguageCenterCacheName = "LanguageCenterCache";

        public LanguageCenterAppService(
            IRepository<LanguageCenter, long> repository,
            ICacheManager cacheManager)
            : base(repository)
        {
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// جلب جميع المعاهد مع الفلترة - Get all language centers with filtering
        /// </summary>
        public override async Task<PagedResultDto<LanguageCenterDto>> GetAllAsync(PagedLanguageCenterRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = ObjectMapper.Map<List<LanguageCenterDto>>(entities);

            return new PagedResultDto<LanguageCenterDto>(totalCount, dtos);
        }

        /// <summary>
        /// جلب معهد واحد - Get single language center
        /// </summary>
        public override async Task<LanguageCenterDto> GetAsync(EntityDto<long> input)
        {
            var center = await Repository.GetAllIncluding(lc => lc.Country, lc => lc.City)
                .FirstOrDefaultAsync(lc => lc.Id == input.Id);

            if (center == null)
                throw new UserFriendlyException("Language center not found");

            return ObjectMapper.Map<LanguageCenterDto>(center);
        }

        /// <summary>
        /// إنشاء معهد جديد - Create language center
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_LanguageCenters_Create)]
        public override async Task<LanguageCenterDto> CreateAsync(CreateLanguageCenterDto input)
        {
            CheckCreatePermission();

            // Validate slug uniqueness
            await ValidateSlugAsync(input.Slug, input.SlugAr);

            var center = ObjectMapper.Map<LanguageCenter>(input);
            await Repository.InsertAsync(center);
            await CurrentUnitOfWork.SaveChangesAsync();

            // Clear cache
            await ClearCacheAsync();

            return ObjectMapper.Map<LanguageCenterDto>(center);
        }

        /// <summary>
        /// تحديث معهد - Update language center
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_LanguageCenters_Edit)]
        public override async Task<LanguageCenterDto> UpdateAsync(UpdateLanguageCenterDto input)
        {
            CheckUpdatePermission();

            var center = await Repository.GetAsync(input.Id);
            
            // Validate slug uniqueness if changed
            if (center.Slug != input.Slug || center.SlugAr != input.SlugAr)
            {
                await ValidateSlugAsync(input.Slug, input.SlugAr, input.Id);
            }

            ObjectMapper.Map(input, center);
            await CurrentUnitOfWork.SaveChangesAsync();

            // Clear cache
            await ClearCacheAsync();

            return ObjectMapper.Map<LanguageCenterDto>(center);
        }

        /// <summary>
        /// حذف معهد - Delete language center
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_LanguageCenters_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            CheckDeletePermission();
            await Repository.DeleteAsync(input.Id);
            await ClearCacheAsync();
        }

        /// <summary>
        /// جلب المعاهد النشطة فقط - Get active language centers only
        /// </summary>
        public async Task<PagedResultDto<LanguageCenterDto>> GetAllActiveAsync(PagedLanguageCenterRequestDto input)
        {
            input.IsActive = true;
            return await GetAllAsync(input);
        }

        /// <summary>
        /// جلب المعاهد المميزة - Get featured language centers
        /// </summary>
        public async Task<ListResultDto<LanguageCenterDto>> GetFeaturedAsync(int count = 10)
        {
            var cacheKey = $"FeaturedLanguageCenters:{count}";

            return await _cacheManager.GetCache(LanguageCenterCacheName).GetAsync(
                cacheKey,
                async (key) =>
                {
                    var centers = await Repository.GetAll()
                        .AsNoTracking()
                        .Where(lc => lc.IsFeatured && lc.IsActive)
                        .OrderByDescending(lc => lc.Rating)
                        .Take(count)
                        .ToListAsync();

                    return new ListResultDto<LanguageCenterDto>(
                        ObjectMapper.Map<List<LanguageCenterDto>>(centers)
                    ) as object;
                }
            ) as ListResultDto<LanguageCenterDto>;
        }

        /// <summary>
        /// جلب معهد حسب Slug - Get language center by slug
        /// </summary>
        public async Task<LanguageCenterDto> GetBySlugAsync(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                throw new UserFriendlyException("Invalid slug");

            var center = await Repository.FirstOrDefaultAsync(lc =>
                (lc.Slug == slug || lc.SlugAr == slug) && lc.IsActive);

            if (center == null)
                throw new UserFriendlyException("Language center not found");

            return ObjectMapper.Map<LanguageCenterDto>(center);
        }

        /// <summary>
        /// Get complete language center details with all related data
        /// </summary>
        public async Task<LanguageCenterDetailDto> GetLanguageCenterDetailAsync(long id)
        {
            var cacheKey = $"LanguageCenterDetail:{id}";

            return await _cacheManager.GetCache(LanguageCenterCacheName).GetAsync(
                cacheKey,
                async (key) =>
                {
                    var center = await Repository.GetAll()
                        .AsNoTracking()
                        .Include(lc => lc.Country)
                        .Include(lc => lc.City)
                        .FirstOrDefaultAsync(lc => lc.Id == id);

                    if (center == null)
                        throw new UserFriendlyException("Language center not found");

                    var detailDto = ObjectMapper.Map<LanguageCenterDetailDto>(center);
                    
                    // TODO: Load courses and FAQs when those services are ready
                    detailDto.TotalCourses = 0;
                    detailDto.TotalPublishedFAQs = 0;

                    return detailDto as object;
                }
            ) as LanguageCenterDetailDto;
        }

        /// <summary>
        /// Get complete language center details by slug
        /// </summary>
        public async Task<LanguageCenterDetailDto> GetLanguageCenterDetailBySlugAsync(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
                throw new UserFriendlyException("Invalid slug");

            var center = await Repository.FirstOrDefaultAsync(lc =>
                (lc.Slug == slug || lc.SlugAr == slug) && lc.IsActive);

            if (center == null)
                throw new UserFriendlyException("Language center not found");

            return await GetLanguageCenterDetailAsync(center.Id);
        }

        /// <summary>
        /// جلب المعاهد حسب الدولة - Get language centers by country
        /// </summary>
        public async Task<ListResultDto<LanguageCenterDto>> GetByCountryAsync(int countryId)
        {
            var centers = await Repository.GetAll()
                .Where(lc => lc.CountryId == countryId && lc.IsActive)
                .OrderBy(lc => lc.Name)
                .ToListAsync();

            return new ListResultDto<LanguageCenterDto>(
                ObjectMapper.Map<List<LanguageCenterDto>>(centers)
            );
        }

        /// <summary>
        /// جلب المعاهد المعتمدة - Get accredited language centers
        /// </summary>
        public async Task<PagedResultDto<LanguageCenterDto>> GetAccreditedAsync(PagedLanguageCenterRequestDto input)
        {
            input.IsAccredited = true;
            return await GetAllAsync(input);
        }

        /// <summary>
        /// تفعيل/إلغاء تفعيل معهد - Toggle active status
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_LanguageCenters_Edit)]
        public async Task ToggleActiveAsync(long id)
        {
            var center = await Repository.GetAsync(id);
            center.IsActive = !center.IsActive;
            await CurrentUnitOfWork.SaveChangesAsync();
            await ClearCacheAsync();
        }

        /// <summary>
        /// جعل معهد مميز - Toggle featured status
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_LanguageCenters_Edit)]
        public async Task ToggleFeaturedAsync(long id)
        {
            var center = await Repository.GetAsync(id);
            center.IsFeatured = !center.IsFeatured;
            await CurrentUnitOfWork.SaveChangesAsync();
            await ClearCacheAsync();
        }

        #region Helper Methods

        protected override IQueryable<LanguageCenter> CreateFilteredQuery(PagedLanguageCenterRequestDto input)
        {
            var query = Repository.GetAllIncluding(lc => lc.Country, lc => lc.City);

            // Keyword search
            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(lc =>
                    lc.Name.Contains(input.Keyword) ||
                    lc.NameAr.Contains(input.Keyword) ||
                    (lc.Country != null && (lc.Country.Name.Contains(input.Keyword) || lc.Country.NameAr.Contains(input.Keyword))) ||
                    (lc.City != null && (lc.City.Name.Contains(input.Keyword) || lc.City.NameAr.Contains(input.Keyword))));
            }

            if (input.CountryId.HasValue)
                query = query.Where(lc => lc.CountryId == input.CountryId.Value);

            if (input.CityId.HasValue)
                query = query.Where(lc => lc.CityId == input.CityId.Value);

            if (input.IsActive.HasValue)
                query = query.Where(lc => lc.IsActive == input.IsActive.Value);

            if (input.IsFeatured.HasValue)
                query = query.Where(lc => lc.IsFeatured == input.IsFeatured.Value);

            if (input.IsAccredited.HasValue)
                query = query.Where(lc => lc.IsAccredited == input.IsAccredited.Value);

            if (input.MinRating.HasValue)
                query = query.Where(lc => lc.Rating >= input.MinRating.Value);

            return query;
        }

        protected override IQueryable<LanguageCenter> ApplySorting(IQueryable<LanguageCenter> query, PagedLanguageCenterRequestDto input)
        {
            if (!string.IsNullOrWhiteSpace(input.Sorting))
                return query.OrderBy(input.Sorting);

            return query.OrderBy(lc => lc.Name);
        }

        private async Task ValidateSlugAsync(string slug, string slugAr, long? excludeId = null)
        {
            if (!string.IsNullOrWhiteSpace(slug))
            {
                var exists = await Repository.GetAll()
                    .AnyAsync(lc => lc.Slug == slug && lc.Id != excludeId);
                
                if (exists)
                    throw new UserFriendlyException($"Slug '{slug}' already exists");
            }

            if (!string.IsNullOrWhiteSpace(slugAr))
            {
                var exists = await Repository.GetAll()
                    .AnyAsync(lc => lc.SlugAr == slugAr && lc.Id != excludeId);
                
                if (exists)
                    throw new UserFriendlyException($"Arabic slug '{slugAr}' already exists");
            }
        }

        private async Task ClearCacheAsync()
        {
            await _cacheManager.GetCache(LanguageCenterCacheName).ClearAsync();
        }

        #endregion
    }
}
