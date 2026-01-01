using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ANLASH.LanguageCenters;
using System;

namespace ANLASH.LanguageCenters.Dto
{
    /// <summary>
    /// Language Center DTO - Data Transfer Object
    /// </summary>
    [AutoMapFrom(typeof(LanguageCenter))]
    public class LanguageCenterDto : FullAuditedEntityDto<long>
    {
        // Basic Info
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string AboutText { get; set; }
        public string AboutTextAr { get; set; }

        // Location
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public int? CityId { get; set; }
        public string CityName { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }

        // Contact
        public string WebsiteUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string WhatsApp { get; set; }

        // Media
        public Guid? LogoBlobId { get; set; }
        public string LogoUrl { get; set; }
        public Guid? CoverImageBlobId { get; set; }
        public string GalleryImages { get; set; }

        // Accreditation
        public bool IsAccredited { get; set; }
        public string AccreditationBody { get; set; }
        public string AccreditationNumber { get; set; }
        public DateTime? AccreditationExpiryDate { get; set; }

        // Registration
        public string RegistrationSteps { get; set; }
        public string RequiredDocuments { get; set; }

        // Accommodation
        public bool ProvidesAccommodation { get; set; }
        public string AccommodationTypes { get; set; }
        public string AccommodationDetails { get; set; }
        public string AccommodationDetailsAr { get; set; }

        // SEO
        public string Slug { get; set; }
        public string SlugAr { get; set; }
        public string MetaDescription { get; set; }
        public string MetaDescriptionAr { get; set; }

        // Display & Status
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public decimal Rating { get; set; }
        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// Create Language Center DTO
    /// </summary>
    [AutoMapTo(typeof(LanguageCenter))]
    public class CreateLanguageCenterDto
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string AboutText { get; set; }
        public string AboutTextAr { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string WebsiteUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string WhatsApp { get; set; }
        public Guid? LogoBlobId { get; set; }
        public string LogoUrl { get; set; }
        public Guid? CoverImageBlobId { get; set; }
        public string GalleryImages { get; set; }
        public bool IsAccredited { get; set; }
        public string AccreditationBody { get; set; }
        public string AccreditationNumber { get; set; }
        public DateTime? AccreditationExpiryDate { get; set; }
        public string RegistrationSteps { get; set; }
        public string RequiredDocuments { get; set; }
        public bool ProvidesAccommodation { get; set; }
        public string AccommodationTypes { get; set; }
        public string AccommodationDetails { get; set; }
        public string AccommodationDetailsAr { get; set; }
        public string Slug { get; set; }
        public string SlugAr { get; set; }
        public string MetaDescription { get; set; }
        public string MetaDescriptionAr { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; }
        public decimal Rating { get; set; }
        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// Update Language Center DTO
    /// </summary>
    [AutoMapTo(typeof(LanguageCenter))]
    public class UpdateLanguageCenterDto : EntityDto<long>
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public string AboutText { get; set; }
        public string AboutTextAr { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public string State { get; set; }
        public string Address { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string WebsiteUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string WhatsApp { get; set; }
        public Guid? LogoBlobId { get; set; }
        public string LogoUrl { get; set; }
        public Guid? CoverImageBlobId { get; set; }
        public string GalleryImages { get; set; }
        public bool IsAccredited { get; set; }
        public string AccreditationBody { get; set; }
        public string AccreditationNumber { get; set; }
        public DateTime? AccreditationExpiryDate { get; set; }
        public string RegistrationSteps { get; set; }
        public string RequiredDocuments { get; set; }
        public bool ProvidesAccommodation { get; set; }
        public string AccommodationTypes { get; set; }
        public string AccommodationDetails { get; set; }
        public string AccommodationDetailsAr { get; set; }
        public string Slug { get; set; }
        public string SlugAr { get; set; }
        public string MetaDescription { get; set; }
        public string MetaDescriptionAr { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public decimal Rating { get; set; }
        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// Paged Language Center Request DTO
    /// </summary>
    public class PagedLanguageCenterRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public int? CountryId { get; set; }
        public int? CityId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsFeatured { get; set; }
        public bool? IsAccredited { get; set; }
        public decimal? MinRating { get; set; }
    }

    /// <summary>
    /// Language Center Detail DTO (with related data)
    /// </summary>
    [AutoMapFrom(typeof(LanguageCenter))]
    public class LanguageCenterDetailDto : LanguageCenterDto
    {
        public ListResultDto<LanguageCourseDto> Courses { get; set; }
        public ListResultDto<LanguageCenterFAQDto> FAQs { get; set; }
        public int TotalCourses { get; set; }
        public int TotalPublishedFAQs { get; set; }
    }
}
