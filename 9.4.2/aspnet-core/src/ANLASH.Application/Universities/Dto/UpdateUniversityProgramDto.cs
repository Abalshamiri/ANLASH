using System;
using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using ANLASH.Universities;

namespace ANLASH.Universities.Dto
{
    /// <summary>
    /// Update University Program DTO
    /// DTO لتحديث برنامج جامعة
    /// </summary>
    public class UpdateUniversityProgramDto : EntityDto<long>
    {
        [Required]
        public long UniversityId { get; set; }
        
        [Required, MaxLength(300)]
        public string Name { get; set; }
        
        [Required, MaxLength(300)]
        public string NameAr { get; set; }
        
        [MaxLength(2000)]
        public string Description { get; set; }
        
        [MaxLength(2000)]
        public string DescriptionAr { get; set; }
        
        [Required]
        public ProgramLevel Level { get; set; }
        
        [Required]
        public StudyMode Mode { get; set; }
        
        [MaxLength(200)]
        public string Field { get; set; }
        
        [MaxLength(200)]
        public string FieldAr { get; set; }
        
        public int DurationYears { get; set; }
        public int? DurationSemesters { get; set; }
        public int? DurationMonths { get; set; }
        
        public int? TotalCredits { get; set; }
        
        public decimal? TuitionFee { get; set; }
        public int? CurrencyId { get; set; }
        
        [MaxLength(50)]
        public string FeeType { get; set; }
        
        public decimal? ApplicationFee { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        
        public string Requirements { get; set; }
        public string RequirementsAr { get; set; }
        
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
        
        [MaxLength(400)]
        public string Slug { get; set; }
        
        [MaxLength(400)]
        public string SlugAr { get; set; }
    }
}
