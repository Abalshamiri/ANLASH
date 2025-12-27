using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using ANLASH.Universities.Dto;

namespace ANLASH.Universities
{
    /// <summary>
    /// University Content Application Service
    /// خدمة تطبيق محتوى الجامعة
    /// </summary>
    public class UniversityContentAppService : AsyncCrudAppService<
        UniversityContent,
        UniversityContentDto,
        long,
        PagedAndSortedResultRequestDto,
        CreateUniversityContentDto,
        UpdateUniversityContentDto>,
        IUniversityContentAppService
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository">University Content Repository</param>
        public UniversityContentAppService(IRepository<UniversityContent, long> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Get all contents for a specific university
        /// الحصول على جميع محتويات جامعة معينة
        /// </summary>
        public async Task<ListResultDto<UniversityContentDto>> GetByUniversityIdAsync(long universityId)
        {
            var contents = await Repository.GetAllListAsync(c =>
                c.UniversityId == universityId &&
                !c.IsDeleted);

            // Sort by DisplayOrder
            var orderedContents = contents.OrderBy(c => c.DisplayOrder).ToList();

            return new ListResultDto<UniversityContentDto>(
                ObjectMapper.Map<List<UniversityContentDto>>(orderedContents)
            );
        }

        /// <summary>
        /// Get content by type for a university
        /// الحصول على محتوى حسب النوع لجامعة معينة
        /// </summary>
        public async Task<UniversityContentDto> GetByTypeAsync(long universityId, UniversityContentType contentType)
        {
            var content = await Repository.FirstOrDefaultAsync(c =>
                c.UniversityId == universityId &&
                c.ContentType == contentType &&
                !c.IsDeleted);

            return ObjectMapper.Map<UniversityContentDto>(content);
        }

        /// <summary>
        /// Reorder contents for a university
        /// إعادة ترتيب محتويات جامعة
        /// </summary>
        public async Task ReorderAsync(long universityId, List<long> contentIds)
        {
            var contents = await Repository.GetAllListAsync(c =>
                c.UniversityId == universityId);

            for (int i = 0; i < contentIds.Count; i++)
            {
                var content = contents.FirstOrDefault(c => c.Id == contentIds[i]);
                if (content != null)
                {
                    content.DisplayOrder = i;
                    await Repository.UpdateAsync(content);
                }
            }
        }

        /// <summary>
        /// Create query with filters
        /// </summary>
        protected override IQueryable<UniversityContent> CreateFilteredQuery(PagedAndSortedResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!string.IsNullOrEmpty(input.Sorting), c => c.IsActive);
        }

        /// <summary>
        /// Apply sorting
        /// </summary>
        protected override IQueryable<UniversityContent> ApplySorting(IQueryable<UniversityContent> query, PagedAndSortedResultRequestDto input)
        {
            // Default sorting by DisplayOrder
            if (string.IsNullOrEmpty(input.Sorting))
            {
                return query.OrderBy(c => c.DisplayOrder);
            }

            return base.ApplySorting(query, input);
        }

        /// <summary>
        /// Map to DTO with additional properties
        /// </summary>
        protected override UniversityContentDto MapToEntityDto(UniversityContent entity)
        {
            var dto = base.MapToEntityDto(entity);

            // Set content type name from enum Display attribute
            dto.ContentTypeName = entity.ContentType.ToString();

            return dto;
        }
    }
}
