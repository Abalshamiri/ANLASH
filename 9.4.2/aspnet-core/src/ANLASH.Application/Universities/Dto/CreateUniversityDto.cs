using Abp.AutoMapper;
using ANLASH.Universities;
using System.ComponentModel.DataAnnotations;

namespace ANLASH.Universities.Dto
{
    /// <summary>
    /// DTO لإنشاء جامعة جديدة - Create University DTO
    /// </summary>
    [AutoMapTo(typeof(University))]
    public class CreateUniversityDto
    {
        // English Fields
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string Description { get; set; }

        // Arabic Fields ✨
        [MaxLength(200)]
        public string NameAr { get; set; }

        [MaxLength(2000)]
        public string DescriptionAr { get; set; }

        // Location
        [Required]
        [MaxLength(100)]
        public string Country { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        // Additional Info
        [Required]
        public UniversityType Type { get; set; }

        [MaxLength(500)]
        public string LogoUrl { get; set; }

        [MaxLength(300)]
        public string WebsiteUrl { get; set; }

        [MaxLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [Range(0, 5)]
        public decimal Rating { get; set; }

        public int? WorldRanking { get; set; }

        public int? EstablishmentYear { get; set; }

        // SEO & Display
        [MaxLength(300)]
        public string Slug { get; set; }

        [MaxLength(300)]
        public string SlugAr { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; }

        public int DisplayOrder { get; set; }
    }
}
