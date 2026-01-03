using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
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

        public override async Task<UniversityContentDto> CreateAsync(CreateUniversityContentDto input)
        {
            try
            {
                CheckCreatePermission();
                Logger.Info($"Creating new Content for University ID: {input.UniversityId}, Type: {input.ContentType}");

                var entity = ObjectMapper.Map<UniversityContent>(input);
                var insertedEntity = await Repository.InsertAsync(entity);
                await CurrentUnitOfWork.SaveChangesAsync();

                Logger.Info($"Successfully created Content with ID: {insertedEntity.Id}");

                return await GetAsync(new EntityDto<long>(insertedEntity.Id));
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error creating Content for University ID: {input.UniversityId}", ex);
                throw new UserFriendlyException(
                    L("ErrorCreatingContent"),
                    "An error occurred while creating the content. Please check the logs for details."
                );
            }
        }

        public override async Task<UniversityContentDto> UpdateAsync(UpdateUniversityContentDto input)
        {
            try
            {
                CheckUpdatePermission();
                Logger.Info($"Updating Content with ID: {input.Id}");

                var entity = await Repository.FirstOrDefaultAsync(input.Id);
                if (entity == null)
                {
                    Logger.Warn($"Content not found with ID: {input.Id}");
                    throw new UserFriendlyException(L("ContentNotFound"));
                }

                ObjectMapper.Map(input, entity);
                await Repository.UpdateAsync(entity);
                await CurrentUnitOfWork.SaveChangesAsync();

                Logger.Info($"Successfully updated Content with ID: {input.Id}");

                return await GetAsync(input);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error updating Content with ID: {input.Id}", ex);
                throw new UserFriendlyException(L("ErrorUpdatingContent"));
            }
        }
    }
}
