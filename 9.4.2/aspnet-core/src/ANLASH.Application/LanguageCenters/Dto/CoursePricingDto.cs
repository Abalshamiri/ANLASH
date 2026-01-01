using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ANLASH.LanguageCenters;

namespace ANLASH.LanguageCenters.Dto
{
    /// <summary>
    /// Course Pricing DTO
    /// </summary>
    [AutoMapFrom(typeof(CoursePricing))]
    public class CoursePricingDto : FullAuditedEntityDto<long>
    {
        public long LanguageCourseId { get; set; }
        public string CourseName { get; set; }
        public int DurationWeeks { get; set; }
        public decimal Fee { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public decimal? FeePerWeek { get; set; }
        public decimal? RegistrationFee { get; set; }
        public decimal? MaterialsFee { get; set; }
        public decimal? ExamFee { get; set; }
        public int? VisaDurationWeeks { get; set; }
        public decimal? VisaProcessingFee { get; set; }
        public bool HasDiscount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? FinalPrice { get; set; }
        public string PromotionDescription { get; set; }
        public string PromotionDescriptionAr { get; set; }
        public bool IsActive { get; set; }
        public bool IsMostPopular { get; set; }
        public int DisplayOrder { get; set; }
        public string Notes { get; set; }
        public string NotesAr { get; set; }
    }

    [AutoMapTo(typeof(CoursePricing))]
    public class CreateCoursePricingDto
    {
        public long LanguageCourseId { get; set; }
        public int DurationWeeks { get; set; }
        public decimal Fee { get; set; }
        public int CurrencyId { get; set; }
        public decimal? FeePerWeek { get; set; }
        public decimal? RegistrationFee { get; set; }
        public decimal? MaterialsFee { get; set; }
        public decimal? ExamFee { get; set; }
        public int? VisaDurationWeeks { get; set; }
        public decimal? VisaProcessingFee { get; set; }
        public bool HasDiscount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? FinalPrice { get; set; }
        public string PromotionDescription { get; set; }
        public string PromotionDescriptionAr { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsMostPopular { get; set; }
        public int DisplayOrder { get; set; }
        public string Notes { get; set; }
        public string NotesAr { get; set; }
    }

    [AutoMapTo(typeof(CoursePricing))]
    public class UpdateCoursePricingDto : EntityDto<long>
    {
        public long LanguageCourseId { get; set; }
        public int DurationWeeks { get; set; }
        public decimal Fee { get; set; }
        public int CurrencyId { get; set; }
        public decimal? FeePerWeek { get; set; }
        public decimal? RegistrationFee { get; set; }
        public decimal? MaterialsFee { get; set; }
        public decimal? ExamFee { get; set; }
        public int? VisaDurationWeeks { get; set; }
        public decimal? VisaProcessingFee { get; set; }
        public bool HasDiscount { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? FinalPrice { get; set; }
        public string PromotionDescription { get; set; }
        public string PromotionDescriptionAr { get; set; }
        public bool IsActive { get; set; }
        public bool IsMostPopular { get; set; }
        public int DisplayOrder { get; set; }
        public string Notes { get; set; }
        public string NotesAr { get; set; }
    }

    public class PagedCoursePricingRequestDto : PagedAndSortedResultRequestDto
    {
        public long? LanguageCourseId { get; set; }
        public int? DurationWeeks { get; set; }
        public bool? IsActive { get; set; }
        public bool? HasDiscount { get; set; }
    }
}
