using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ANLASH.Universities;
using System;

namespace ANLASH.Universities.Dto
{
    /// <summary>
    /// University DTO - Data Transfer Object
    /// </summary>
    [AutoMapFrom(typeof(University))]
    public class UniversityDto : FullAuditedEntityDto
    {
        // English Fields
        public string Name { get; set; }
        public string Description { get; set; }

        // Arabic Fields ✨
        public string NameAr { get; set; }
        public string DescriptionAr { get; set; }

        // Location
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        // Additional Info
        public UniversityType Type { get; set; }
        public string LogoUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public decimal Rating { get; set; }
        public int? WorldRanking { get; set; }
        public int? EstablishmentYear { get; set; }

        // SEO & Display
        public string Slug { get; set; }
        public string SlugAr { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
    }
}
