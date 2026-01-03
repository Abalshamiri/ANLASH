using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using ANLASH.Universities.Dto;
using Microsoft.EntityFrameworkCore;

namespace ANLASH.Universities
{
    /// <summary>
    /// University FAQ Application Service
    /// خدمة تطبيق الأسئلة الشائعة للجامعة
    /// </summary>
    public class UniversityFAQAppService : ApplicationService, IUniversityFAQAppService
    {
        private readonly IRepository<UniversityFAQ, long> _faqRepository;
        private readonly IRepository<University, long> _universityRepository;

        public UniversityFAQAppService(
            IRepository<UniversityFAQ, long> faqRepository,
            IRepository<University, long> universityRepository)
        {
            _faqRepository = faqRepository;
            _universityRepository = universityRepository;
        }

        public async Task<UniversityFAQDto> GetAsync(long id)
        {
            var faq = await _faqRepository.GetAll()
                .Include(f => f.University)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (faq == null)
            {
                throw new UserFriendlyException("FAQ not found");
            }

            var dto = ObjectMapper.Map<UniversityFAQDto>(faq);
            
            if (faq.University != null)
            {
                dto.UniversityName = faq.University.Name;
                dto.UniversityNameAr = faq.University.NameAr;
            }

            return dto;
        }

        public async Task<ListResultDto<UniversityFAQDto>> GetByUniversityAsync(long universityId)
        {
            var faqs = await _faqRepository.GetAll()
                .Where(f => f.UniversityId == universityId && !f.IsDeleted)
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();

            return new ListResultDto<UniversityFAQDto>(
                ObjectMapper.Map<List<UniversityFAQDto>>(faqs)
            );
        }

        public async Task<ListResultDto<UniversityFAQDto>> GetPublishedByUniversityAsync(long universityId)
        {
            var faqs = await _faqRepository.GetAll()
                .Where(f => f.UniversityId == universityId && f.IsPublished && !f.IsDeleted)
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();

            return new ListResultDto<UniversityFAQDto>(
                ObjectMapper.Map<List<UniversityFAQDto>>(faqs)
            );
        }

        public async Task<UniversityFAQDto> CreateAsync(CreateUniversityFAQDto input)
        {
            try
            {
                Console.WriteLine($"===== FAQ CREATE START =====");
                Console.WriteLine($"Step 1: Logging start - University ID: {input.UniversityId}");
                Logger.Info($"Creating new FAQ for University ID: {input.UniversityId}");

                Console.WriteLine($"Step 2: Checking university exists");
                // Validate university exists
                var university = await _universityRepository.FirstOrDefaultAsync(input.UniversityId);
                if (university == null)
                {
                    Console.WriteLine($"Step 2 FAILED: University not found");
                    Logger.Warn($"University not found with ID: {input.UniversityId}");
                    throw new UserFriendlyException(
                        L("UniversityNotFound"),
                        $"University with ID {input.UniversityId} does not exist."
                    );
                }
                Console.WriteLine($"Step 2 SUCCESS: University found - {university.Name}");

                Console.WriteLine($"Step 3: Mapping DTO to entity");
                // Map DTO to entity
                var faq = ObjectMapper.Map<UniversityFAQ>(input);
                Console.WriteLine($"Step 3 SUCCESS: Mapped successfully");
                
                Console.WriteLine($"Step 4: Inserting into repository");
                // Insert into database
                var insertedFaq = await _faqRepository.InsertAsync(faq);
                Console.WriteLine($"Step 4 SUCCESS: Inserted with ID: {insertedFaq.Id}");
                
                Console.WriteLine($"Step 5: Saving changes");
                await CurrentUnitOfWork.SaveChangesAsync();
                Console.WriteLine($"Step 5 SUCCESS: Changes saved");

                Console.WriteLine($"Step 6: Mapping result");
                Logger.Info($"Successfully created FAQ with ID: {insertedFaq.Id} for University: {university.Name}");

                // Map to DTO
                var result = ObjectMapper.Map<UniversityFAQDto>(insertedFaq);
                result.UniversityName = university.Name;
                result.UniversityNameAr = university.NameAr;
                
                Console.WriteLine($"Step 6 SUCCESS: Mapped to DTO");
                Console.WriteLine($"===== FAQ CREATE END (SUCCESS) =====");
                
                return result;
            }
            catch (UserFriendlyException)
            {
                Console.WriteLine($"===== FAQ CREATE END (USER FRIENDLY EXCEPTION) =====");
                // Re-throw user-friendly exceptions as-is
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"===== FAQ CREATE ERROR =====");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                Console.WriteLine($"InnerException: {ex.InnerException?.Message}");
                Console.WriteLine($"============================");
                
                Logger.Error($"Error creating FAQ for University ID: {input.UniversityId}", ex);
                throw new UserFriendlyException(
                    L("ErrorCreatingFAQ"),
                    "An error occurred while creating the FAQ. Please check the logs for details."
                );
            }
        }

        public async Task<UniversityFAQDto> UpdateAsync(UpdateUniversityFAQDto input)
        {
            try
            {
                Logger.Info($"Updating FAQ with ID: {input.Id}");

                var faq = await _faqRepository.FirstOrDefaultAsync(input.Id);
                if (faq == null)
                {
                    Logger.Warn($"FAQ not found with ID: {input.Id}");
                    throw new UserFriendlyException(L("FAQNotFound"));
                }

                ObjectMapper.Map(input, faq);
                await _faqRepository.UpdateAsync(faq);
                await CurrentUnitOfWork.SaveChangesAsync();

                Logger.Info($"Successfully updated FAQ with ID: {input.Id}");

                return await GetAsync(faq.Id);
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error updating FAQ with ID: {input.Id}", ex);
                throw new UserFriendlyException(L("ErrorUpdatingFAQ"));
            }
        }

        public async Task DeleteAsync(long id)
        {
            try
            {
                Logger.Info($"Deleting FAQ with ID: {id}");
                
                var faq = await _faqRepository.FirstOrDefaultAsync(id);
                if (faq == null)
                {
                    Logger.Warn($"FAQ not found with ID: {id}");
                    throw new UserFriendlyException(L("FAQNotFound"));
                }

                await _faqRepository.DeleteAsync(id);
                await CurrentUnitOfWork.SaveChangesAsync();

                Logger.Info($"Successfully deleted FAQ with ID: {id}");
            }
            catch (UserFriendlyException)
            {
                throw;
            }
            catch (Exception ex)
            {
                Logger.Error($"Error deleting FAQ with ID: {id}", ex);
                throw new UserFriendlyException(L("ErrorDeletingFAQ"));
            }
        }

        public async Task ReorderAsync(List<FAQOrderDto> input)
        {
            foreach (var item in input)
            {
                var faq = await _faqRepository.GetAsync(item.Id);
                faq.DisplayOrder = item.DisplayOrder;
                await _faqRepository.UpdateAsync(faq);
            }
        }

        public async Task<UniversityFAQDto> TogglePublishAsync(long id)
        {
            var faq = await _faqRepository.GetAsync(id);
            faq.IsPublished = !faq.IsPublished;
            await _faqRepository.UpdateAsync(faq);

            return await GetAsync(id);
        }
    }
}
