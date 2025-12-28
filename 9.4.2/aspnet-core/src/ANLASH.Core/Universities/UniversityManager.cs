using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ANLASH.Universities
{
    /// <summary>
    /// مدير نطاق الجامعات - University Domain Manager
    /// <para>Handles business logic for universities</para>
    /// </summary>
    public class UniversityManager : DomainService
    {
        private readonly IRepository<University, long> _universityRepository;

        public UniversityManager(IRepository<University, long> universityRepository)
        {
            _universityRepository = universityRepository;
        }

        /// <summary>
        /// إنشاء جامعة جديدة مع التحقق - Create university with validation
        /// </summary>
        public async Task<University> CreateAsync(University university)
        {
            // تعطيل duplicate name check مؤقتاً
            // يمكن تفعيله لاحقاً إذا لزم الأمر
            
            // Generate slug if not provided
            if (string.IsNullOrWhiteSpace(university.Slug))
            {
                university.Slug = GenerateSlug(university.Name);
            }

            if (string.IsNullOrWhiteSpace(university.SlugAr) && !string.IsNullOrWhiteSpace(university.NameAr))
            {
                university.SlugAr = GenerateSlug(university.NameAr);
            }

            // Fix rating if invalid
            if (university.Rating < 0 || university.Rating > 5)
            {
                university.Rating = 0;
            }

            return await _universityRepository.InsertAsync(university);
        }

        /// <summary>
        /// تحديث جامعة - Update university
        /// </summary>
        public async Task<University> UpdateAsync(University university)
        {
            // تعطيل duplicate name check مؤقتاً
            
            return await _universityRepository.UpdateAsync(university);
        }

        /// <summary>
        /// حذف جامعة - Delete university
        /// </summary>
        public async Task DeleteAsync(long id)
        {
            var university = await _universityRepository.GetAsync(id);
            await _universityRepository.DeleteAsync(university);
        }

        /// <summary>
        /// توليد رابط ودي - Generate SEO-friendly slug
        /// </summary>
        private string GenerateSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            return text
                .ToLowerInvariant()
                .Replace(" ", "-")
                .Replace("&", "and")
                .Replace("--", "-")
                .Trim('-');
        }

        /// <summary>
        /// التحقق من صحة الرابط - Validate unique slug
        /// </summary>
        public async Task<bool> IsSlugUniqueAsync(string slug, long? excludeId = null)
        {
            var query = _universityRepository.GetAll()
                .Where(u => u.Slug == slug && !u.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(u => u.Id != excludeId.Value);
            }

            return !await query.AnyAsync();
        }
    }
}
