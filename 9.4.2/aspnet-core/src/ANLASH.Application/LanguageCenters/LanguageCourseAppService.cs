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
    /// خدمة التطبيق للدورات اللغوية - Language Course Application Service
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_LanguageCourses)]
    public class LanguageCourseAppService : AsyncCrudAppService<LanguageCourse, LanguageCourseDto, long, PagedLanguageCourseRequestDto, CreateLanguageCourseDto, UpdateLanguageCourseDto>, ILanguageCourseAppService
    {
        private readonly IRepository<LanguageCenter, long> _languageCenterRepository;

        public LanguageCourseAppService(
            IRepository<LanguageCourse, long> repository,
            IRepository<LanguageCenter, long> languageCenterRepository)
            : base(repository)
        {
            _languageCenterRepository = languageCenterRepository;
        }

        public override async Task<PagedResultDto<LanguageCourseDto>> GetAllAsync(PagedLanguageCourseRequestDto input)
        {
            var query = CreateFilteredQuery(input);
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = ObjectMapper.Map<List<LanguageCourseDto>>(entities);

            return new PagedResultDto<LanguageCourseDto>(totalCount, dtos);
        }

        [AbpAuthorize(PermissionNames.Pages_LanguageCourses_Create)]
        public override async Task<LanguageCourseDto> CreateAsync(CreateLanguageCourseDto input)
        {
            CheckCreatePermission();

            // Validate language center exists
            var centerExists = await _languageCenterRepository.GetAll()
                .AnyAsync(lc => lc.Id == input.LanguageCenterId);

            if (!centerExists)
                throw new UserFriendlyException("Language center not found");

            var course = ObjectMapper.Map<LanguageCourse>(input);
            await Repository.InsertAsync(course);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<LanguageCourseDto>(course);
        }

        [AbpAuthorize(PermissionNames.Pages_LanguageCourses_Edit)]
        public override async Task<LanguageCourseDto> UpdateAsync(UpdateLanguageCourseDto input)
        {
            CheckUpdatePermission();

            var course = await Repository.GetAsync(input.Id);
            ObjectMapper.Map(input, course);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<LanguageCourseDto>(course);
        }

        [AbpAuthorize(PermissionNames.Pages_LanguageCourses_Delete)]
        public override async Task DeleteAsync(EntityDto<long> input)
        {
            CheckDeletePermission();
            await Repository.DeleteAsync(input.Id);
        }

        /// <summary>
        /// جلب جميع الدورات لمعهد معين
        /// </summary>
        public async Task<ListResultDto<LanguageCourseDto>> GetByLanguageCenterAsync(long languageCenterId)
        {
            var courses = await Repository.GetAll()
                .Where(c => c.LanguageCenterId == languageCenterId)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            return new ListResultDto<LanguageCourseDto>(
                ObjectMapper.Map<List<LanguageCourseDto>>(courses)
            );
        }

        /// <summary>
        /// جلب الدورات النشطة لمعهد
        /// </summary>
        public async Task<ListResultDto<LanguageCourseDto>> GetActiveByLanguageCenterAsync(long languageCenterId)
        {
            var courses = await Repository.GetAll()
                .Where(c => c.LanguageCenterId == languageCenterId && c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            return new ListResultDto<LanguageCourseDto>(
                ObjectMapper.Map<List<LanguageCourseDto>>(courses)
            );
        }

        /// <summary>
        /// جلب الدورات حسب النوع
        /// </summary>
        public async Task<PagedResultDto<LanguageCourseDto>> GetByCourseTypeAsync(CourseType courseType, PagedLanguageCourseRequestDto input)
        {
            var query = CreateFilteredQuery(input)
                .Where(c => c.CourseType == courseType);
            
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = ObjectMapper.Map<List<LanguageCourseDto>>(entities);
            
            return new PagedResultDto<LanguageCourseDto>(totalCount, dtos);
        }

        /// <summary>
        /// جلب الدورات حسب المستوى
        /// </summary>
        public async Task<PagedResultDto<LanguageCourseDto>> GetByLevelAsync(CourseLevel level, PagedLanguageCourseRequestDto input)
        {
            var query = CreateFilteredQuery(input)
                .Where(c => c.Level == level);
            
            var totalCount = await AsyncQueryableExecuter.CountAsync(query);
            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);
            
            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = ObjectMapper.Map<List<LanguageCourseDto>>(entities);
            
            return new PagedResultDto<LanguageCourseDto>(totalCount, dtos);
        }

        /// <summary>
        /// جلب الدورات المميزة
        /// </summary>
        public async Task<ListResultDto<LanguageCourseDto>> GetFeaturedAsync(int count = 10)
        {
            var courses = await Repository.GetAll()
                .Where(c => c.IsFeatured && c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .Take(count)
                .ToListAsync();

            return new ListResultDto<LanguageCourseDto>(
                ObjectMapper.Map<List<LanguageCourseDto>>(courses)
            );
        }

        /// <summary>
        /// تفعيل/إلغاء تفعيل دورة
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_LanguageCourses_Edit)]
        public async Task ToggleActiveAsync(long id)
        {
            var course = await Repository.GetAsync(id);
            course.IsActive = !course.IsActive;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        #region Helper Methods

        protected override IQueryable<LanguageCourse> CreateFilteredQuery(PagedLanguageCourseRequestDto input)
        {
            var query = Repository.GetAll();

            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(c =>
                    c.CourseName.Contains(input.Keyword) ||
                    c.CourseNameAr.Contains(input.Keyword));
            }

            if (input.LanguageCenterId.HasValue)
                query = query.Where(c => c.LanguageCenterId == input.LanguageCenterId.Value);

            if (input.IsActive.HasValue)
                query = query.Where(c => c.IsActive == input.IsActive.Value);

            if (input.IsFeatured.HasValue)
                query = query.Where(c => c.IsFeatured == input.IsFeatured.Value);

            return query;
        }

        protected override IQueryable<LanguageCourse> ApplySorting(IQueryable<LanguageCourse> query, PagedLanguageCourseRequestDto input)
        {
            if (!string.IsNullOrWhiteSpace(input.Sorting))
                return query.OrderBy(input.Sorting);

            return query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.CourseName);
        }

        #endregion
    }
}
