using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using ANLASH.Authorization;
using ANLASH.Universities.Dto;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ANLASH.Universities
{
    /// <summary>
    /// خدمة التطبيق للجامعات - University Application Service
    /// <para>Handles all university operations with bilingual support</para>
    /// </summary>
    [AbpAuthorize(PermissionNames.Pages_Universities)]
    public class UniversityAppService : AsyncCrudAppService<University, UniversityDto, int, PagedUniversityRequestDto, CreateUniversityDto, UpdateUniversityDto>, IUniversityAppService
    {
        private readonly UniversityManager _universityManager;

        public UniversityAppService(
            IRepository<University, int> repository,
            UniversityManager universityManager)
            : base(repository)
        {
            _universityManager = universityManager;
        }

        /// <summary>
        /// جلب جميع الجامعات مع الفلترة - Get all universities with filtering
        /// </summary>
        public override async Task<PagedResultDto<UniversityDto>> GetAllAsync(PagedUniversityRequestDto input)
        {
            var query = CreateFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            // Apply sorting
            query = ApplySorting(query, input);

            // Apply paging
            query = ApplyPaging(query, input);

            var entities = await AsyncQueryableExecuter.ToListAsync(query);
            var dtos = ObjectMapper.Map<System.Collections.Generic.List<UniversityDto>>(entities);

            return new PagedResultDto<UniversityDto>(totalCount, dtos);
        }

        /// <summary>
        /// جلب جامعة واحدة - Get single university
        /// </summary>
        public override async Task<UniversityDto> GetAsync(EntityDto<int> input)
        {
            var university = await Repository.GetAsync(input.Id);
            return ObjectMapper.Map<UniversityDto>(university);
        }

        /// <summary>
        /// إنشاء جامعة جديدة - Create university
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_Universities_Create)]
        public override async Task<UniversityDto> CreateAsync(CreateUniversityDto input)
        {
            CheckCreatePermission();

            var university = ObjectMapper.Map<University>(input);
            
            await _universityManager.CreateAsync(university);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<UniversityDto>(university);
        }

        /// <summary>
        /// تحديث جامعة - Update university
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_Universities_Edit)]
        public override async Task<UniversityDto> UpdateAsync(UpdateUniversityDto input)
        {
            CheckUpdatePermission();

            var university = await Repository.GetAsync(input.Id);
            ObjectMapper.Map(input, university);

            await _universityManager.UpdateAsync(university);
            await CurrentUnitOfWork.SaveChangesAsync();

            return ObjectMapper.Map<UniversityDto>(university);
        }

        /// <summary>
        /// حذف جامعة - Delete university
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_Universities_Delete)]
        public override async Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();
            await _universityManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// جلب الجامعات النشطة فقط - Get active universities only
        /// </summary>
        public async Task<PagedResultDto<UniversityDto>> GetAllActiveAsync(PagedUniversityRequestDto input)
        {
            input.IsActive = true;
            return await GetAllAsync(input);
        }

        /// <summary>
        /// جلب الجامعات المميزة - Get featured universities
        /// </summary>
        public async Task<ListResultDto<UniversityDto>> GetFeaturedAsync(int count = 10)
        {
            var universities = await Repository.GetAll()
                .Where(u => u.IsFeatured && u.IsActive)
                .OrderByDescending(u => u.Rating)
                .Take(count)
                .ToListAsync();

            return new ListResultDto<UniversityDto>(
                ObjectMapper.Map<System.Collections.Generic.List<UniversityDto>>(universities)
            );
        }

        /// <summary>
        /// جلب جامعة حسب Slug - Get university by slug
        /// </summary>
        public async Task<UniversityDto> GetBySlugAsync(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new UserFriendlyException(L("Universities:InvalidSlug"));
            }

            var university = await Repository.FirstOrDefaultAsync(u => 
                (u.Slug == slug || u.SlugAr == slug) && u.IsActive);

            if (university == null)
            {
                throw new UserFriendlyException(L("Universities:NotFound"));
            }

            return ObjectMapper.Map<UniversityDto>(university);
        }

        /// <summary>
        /// تفعيل/إلغاء تفعيل جامعة - Toggle university active status
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_Universities_Edit)]
        public async Task ToggleActiveAsync(int id)
        {
            var university = await Repository.GetAsync(id);
            university.IsActive = !university.IsActive;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// جعل جامعة مميزة أو إلغاء تمييزها - Toggle featured status
        /// </summary>
        [AbpAuthorize(PermissionNames.Pages_Universities_Edit)]
        public async Task ToggleFeaturedAsync(int id)
        {
            var university = await Repository.GetAsync(id);
            university.IsFeatured = !university.IsFeatured;
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        #region Helper Methods

        /// <summary>
        /// إنشاء استعلام مفلتر - Create filtered query
        /// </summary>
        protected override IQueryable<University> CreateFilteredQuery(PagedUniversityRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.SearchTerm),
                    u => u.Name.Contains(input.SearchTerm) || 
                         u.NameAr.Contains(input.SearchTerm) ||
                         u.Country.Contains(input.SearchTerm) ||
                         u.City.Contains(input.SearchTerm))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Country),
                    u => u.Country == input.Country)
                .WhereIf(!string.IsNullOrWhiteSpace(input.City),
                    u => u.City == input.City)
                .WhereIf(input.Type.HasValue,
                    u => u.Type == input.Type.Value)
                .WhereIf(input.IsFeatured.HasValue,
                    u => u.IsFeatured == input.IsFeatured.Value)
                .WhereIf(input.IsActive.HasValue,
                    u => u.IsActive == input.IsActive.Value)
                .WhereIf(input.MinRating.HasValue,
                    u => u.Rating >= input.MinRating.Value);
        }

        /// <summary>
        /// تطبيق الترتيب - Apply sorting
        /// </summary>
        protected override IQueryable<University> ApplySorting(IQueryable<University> query, PagedUniversityRequestDto input)
        {
            if (!string.IsNullOrWhiteSpace(input.OrderBy))
            {
                var direction = input.IsDescending ? "DESC" : "ASC";
                return query.OrderBy($"{input.OrderBy} {direction}");
            }

            // Default sorting
            return query.OrderBy(u => u.Name);
        }

        #endregion
    }
}
