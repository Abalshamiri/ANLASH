using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using ANLASH.Universities;

namespace ANLASH.Universities.Dto
{
    /// <summary>
    /// University Program DTO
    /// DTO لبرنامج الجامعة
    /// </summary>
    public class UniversityProgramDto : FullAuditedEntityDto<long>
    {
        // University Info
        public long UniversityId { get; set; }
        public string UniversityName { get; set; }
        public string UniversityNameAr { get; set; }
        
        // Program Info
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string NameAr { get; set; }
        
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        
        // Classification
        public ProgramLevel Level { get; set; }
        public string LevelName { get; set; }
        public StudyMode Mode { get; set; }
        public string ModeName { get; set; }
        
        public string Field { get; set; }
        public string FieldAr { get; set; }
        
        // Duration
        public int DurationYears { get; set; }
        public int? DurationSemesters { get; set; }
        public int? DurationMonths { get; set; }
        
        // Credits
        public int? TotalCredits { get; set; }
        
        // Tuition Fees
        public decimal? TuitionFee { get; set; }
        public int? CurrencyId { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencySymbol { get; set; }
        public string FeeType { get; set; }
        
        // Application
        public decimal? ApplicationFee { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        
        // Requirements
        public string Requirements { get; set; }
        public string RequirementsAr { get; set; }
        
        // Status & Display
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
        
        // SEO
        public string Slug { get; set; }
        public string SlugAr { get; set; }
    }
}
