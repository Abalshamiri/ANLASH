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
            // Validate university exists
            var university = await _universityRepository.GetAsync(input.UniversityId);

            var faq = ObjectMapper.Map<UniversityFAQ>(input);
            await _faqRepository.InsertAsync(faq);
            await CurrentUnitOfWork.SaveChangesAsync();

            return await GetAsync(faq.Id);
        }

        public async Task<UniversityFAQDto> UpdateAsync(UpdateUniversityFAQDto input)
        {
            var faq = await _faqRepository.GetAsync(input.Id);
            ObjectMapper.Map(input, faq);
            await _faqRepository.UpdateAsync(faq);

            return await GetAsync(faq.Id);
        }

        public async Task DeleteAsync(long id)
        {
            await _faqRepository.DeleteAsync(id);
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
