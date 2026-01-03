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
using Microsoft.EntityFrameworkCore;

namespace ANLASH.Universities
{
    /// <summary>
    /// University Program Application Service
    /// خدمة تطبيق برامج الجامعة
    /// </summary>
    public class UniversityProgramAppService : AsyncCrudAppService<
        UniversityProgram,
        UniversityProgramDto,
        long,
        PagedAndSortedResultRequestDto,
        CreateUniversityProgramDto,
        UpdateUniversityProgramDto>,
        IUniversityProgramAppService
    {
        public UniversityProgramAppService(IRepository<UniversityProgram, long> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// Get all programs for a specific university
        /// </summary>
        public async Task<ListResultDto<UniversityProgramDto>> GetByUniversityIdAsync(long universityId)
        {
            var programs = await Repository.GetAll()
                .Include(p => p.University)
                .Include(p => p.Currency)
                .Where(p => p.UniversityId == universityId && !p.IsDeleted)
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();

            return new ListResultDto<UniversityProgramDto>(
                ObjectMapper.Map<List<UniversityProgramDto>>(programs)
            );
        }

        /// <summary>
        /// Get programs by level
        /// </summary>
        public async Task<PagedResultDto<UniversityProgramDto>> GetByLevelAsync(
            ProgramLevel level,
            PagedAndSortedResultRequestDto input)
        {
            var query = Repository.GetAll()
                .Include(p => p.University)
                .Include(p => p.Currency)
                .Where(p => p.Level == level && !p.IsDeleted);

            var totalCount = await query.CountAsync();

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var programs = await query.ToListAsync();

            return new PagedResultDto<UniversityProgramDto>(
                totalCount,
                ObjectMapper.Map<List<UniversityProgramDto>>(programs)
            );
        }

        /// <summary>
        /// Get featured programs
        /// </summary>
        public async Task<ListResultDto<UniversityProgramDto>> GetFeaturedAsync(int maxCount = 10)
        {
            var programs = await Repository.GetAll()
                .Include(p => p.University)
                .Include(p => p.Currency)
                .Where(p => p.IsFeatured && p.IsActive && !p.IsDeleted)
                .OrderBy(p => p.DisplayOrder)
                .Take(maxCount)
                .ToListAsync();

            return new ListResultDto<UniversityProgramDto>(
                ObjectMapper.Map<List<UniversityProgramDto>>(programs)
            );
        }

        /// <summary>
        /// Toggle featured status
        /// </summary>
        public async Task ToggleFeaturedAsync(long id)
        {
            var program = await Repository.GetAsync(id);
            program.IsFeatured = !program.IsFeatured;
            await Repository.UpdateAsync(program);
        }

        protected override IQueryable<UniversityProgram> CreateFilteredQuery(PagedAndSortedResultRequestDto input)
        {
            return Repository.GetAll()
                .Include(p => p.University)
                .Include(p => p.Currency)
                .WhereIf(!string.IsNullOrEmpty(input.Sorting), p => p.IsActive);
        }

        protected override IQueryable<UniversityProgram> ApplySorting(
            IQueryable<UniversityProgram> query,
            PagedAndSortedResultRequestDto input)
        {
            if (string.IsNullOrEmpty(input.Sorting))
            {
                return query.OrderBy(p => p.DisplayOrder).ThenBy(p => p.Name);
            }

            return base.ApplySorting(query, input);
        }

        protected override UniversityProgramDto MapToEntityDto(UniversityProgram entity)
        {
            var dto = base.MapToEntityDto(entity);

            // Set enum display names
            dto.LevelName = entity.Level.ToString();
            dto.ModeName = entity.Mode.ToString();

            // Set university info
            if (entity.University != null)
            {
                dto.UniversityName = entity.University.Name;
                dto.UniversityNameAr = entity.University.NameAr;
            }

            // Set currency info
            if (entity.Currency != null)
            {
                dto.CurrencyCode = entity.Currency.Code;
                dto.CurrencySymbol = entity.Currency.Symbol;
            }

            return dto;
        }

        public override async Task<UniversityProgramDto> CreateAsync(CreateUniversityProgramDto input)
        {
            try
            {
                CheckCreatePermission();
                Logger.Info($"Creating new Program for University ID: {input.UniversityId}");

                var entity = ObjectMapper.Map<UniversityProgram>(input);
                var insertedEntity = await Repository.InsertAsync(entity);
                await CurrentUnitOfWork.SaveChangesAsync();

                Logger.Info($"Successfully created Program with ID: {insertedEntity.Id}");

                return await GetAsync(new EntityDto<long>(insertedEntity.Id));
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error creating Program for University ID: {input.UniversityId}", ex);
                throw new UserFriendlyException(
                    L("ErrorCreatingProgram"),
                    "An error occurred while creating the program. Please check the logs for details."
                );
            }
        }

        public override async Task<UniversityProgramDto> UpdateAsync(UpdateUniversityProgramDto input)
        {
            try
            {
                CheckUpdatePermission();
                Logger.Info($"Updating Program with ID: {input.Id}");

                var entity = await Repository.FirstOrDefaultAsync(input.Id);
                if (entity == null)
                {
                    Logger.Warn($"Program not found with ID: {input.Id}");
                    throw new UserFriendlyException(L("ProgramNotFound"));
                }

                ObjectMapper.Map(input, entity);
                await Repository.UpdateAsync(entity);
                await CurrentUnitOfWork.SaveChangesAsync();

                Logger.Info($"Successfully updated Program with ID: {input.Id}");

                return await GetAsync(input);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error updating Program with ID: {input.Id}", ex);
                throw new UserFriendlyException(L("ErrorUpdatingProgram"));
            }
        }
    }
}
