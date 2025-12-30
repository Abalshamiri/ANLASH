using System;
using System.ComponentModel.DataAnnotations;
using ANLASH.Universities;

namespace ANLASH.Universities.Dto
{
    /// <summary>
    /// Create University Program DTO
    /// DTO لإنشاء برنامج جامعة جديد
    /// </summary>
    public class CreateUniversityProgramDto
    {
        [Required]
        public long UniversityId { get; set; }
        
        [Required(ErrorMessage = "Program name is required | اسم البرنامج مطلوب")]
        [MaxLength(300)]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "Arabic program name is required | اسم البرنامج بالعربية مطلوب")]
        [MaxLength(300)]
        public string NameAr { get; set; }
        
        [MaxLength(2000)]
        public string Description { get; set; }
        
        [MaxLength(2000)]
        public string DescriptionAr { get; set; }
        
        [Required(ErrorMessage = "Program level is required | مستوى البرنامج مطلوب")]
        public ProgramLevel Level { get; set; }
        
        [Required(ErrorMessage = "Study mode is required | نوع الدراسة مطلوب")]
        public StudyMode Mode { get; set; }
        
        [MaxLength(200)]
        public string Field { get; set; }
        
        [MaxLength(200)]
        public string FieldAr { get; set; }
        
        [Range(1, 10, ErrorMessage = "Duration must be between 1-10 years | المدة يجب أن تكون بين 1-10 سنوات")]
        public int DurationYears { get; set; }
        public int? DurationSemesters { get; set; }
        public int? DurationMonths { get; set; }
        
        public int? TotalCredits { get; set; }
        
        [Range(0, double.MaxValue, ErrorMessage = "Tuition fee must be positive | الرسوم يجب أن تكون موجبة")]
        public decimal? TuitionFee { get; set; }
        public int? CurrencyId { get; set; }
        
        [MaxLength(50)]
        public string FeeType { get; set; }
        
        public decimal? ApplicationFee { get; set; }
        public DateTime? ApplicationDeadline { get; set; }
        
        public string Requirements { get; set; }
        public string RequirementsAr { get; set; }
        
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
        
        [MaxLength(400)]
        public string Slug { get; set; }
        
        [MaxLength(400)]
        public string SlugAr { get; set; }
    }
}
