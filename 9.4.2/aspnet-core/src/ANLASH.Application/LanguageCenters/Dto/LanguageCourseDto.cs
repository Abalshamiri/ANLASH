using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ANLASH.LanguageCenters;

namespace ANLASH.LanguageCenters.Dto
{
    /// <summary>
    /// Language Course DTO
    /// </summary>
    [AutoMapFrom(typeof(LanguageCourse))]
    public class LanguageCourseDto : FullAuditedEntityDto<long>
    {
        public long LanguageCenterId { get; set; }
        public string LanguageCenterName { get; set; }
        public string CourseName { get; set; }
        public string CourseNameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public CourseType CourseType { get; set; }
        public CourseLevel Level { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? ClassSize { get; set; }
        public int? AverageClassSize { get; set; }
        public int? HoursPerWeek { get; set; }
        public int? LessonsPerWeek { get; set; }
        public int? LessonDurationMinutes { get; set; }
        public string StartDates { get; set; }
        public int? MinDurationWeeks { get; set; }
        public int? MaxDurationWeeks { get; set; }
        public bool IncludesMaterials { get; set; }
        public bool IncludesCertificate { get; set; }
        public string CertificateType { get; set; }
        public bool IncludesPlacementTest { get; set; }
        public string Highlights { get; set; }
        public string HighlightsAr { get; set; }
        public string TargetAudience { get; set; }
        public string TargetAudienceAr { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
    }

    [AutoMapTo(typeof(LanguageCourse))]
    public class CreateLanguageCourseDto
    {
        public long LanguageCenterId { get; set; }
        public string CourseName { get; set; }
        public string CourseNameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public CourseType CourseType { get; set; }
        public CourseLevel Level { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? ClassSize { get; set; }
        public int? AverageClassSize { get; set; }
        public int? HoursPerWeek { get; set; }
        public int? LessonsPerWeek { get; set; }
        public int? LessonDurationMinutes { get; set; }
        public string StartDates { get; set; }
        public int? MinDurationWeeks { get; set; }
        public int? MaxDurationWeeks { get; set; }
        public bool IncludesMaterials { get; set; }
        public bool IncludesCertificate { get; set; }
        public string CertificateType { get; set; }
        public bool IncludesPlacementTest { get; set; }
        public string Highlights { get; set; }
        public string HighlightsAr { get; set; }
        public string TargetAudience { get; set; }
        public string TargetAudienceAr { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
    }

    [AutoMapTo(typeof(LanguageCourse))]
    public class UpdateLanguageCourseDto : EntityDto<long>
    {
        public long LanguageCenterId { get; set; }
        public string CourseName { get; set; }
        public string CourseNameAr { get; set; }
        public string Description { get; set; }
        public string DescriptionAr { get; set; }
        public CourseType CourseType { get; set; }
        public CourseLevel Level { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public int? ClassSize { get; set; }
        public int? AverageClassSize { get; set; }
        public int? HoursPerWeek { get; set; }
        public int? LessonsPerWeek { get; set; }
        public int? LessonDurationMinutes { get; set; }
        public string StartDates { get; set; }
        public int? MinDurationWeeks { get; set; }
        public int? MaxDurationWeeks { get; set; }
        public bool IncludesMaterials { get; set; }
        public bool IncludesCertificate { get; set; }
        public string CertificateType { get; set; }
        public bool IncludesPlacementTest { get; set; }
        public string Highlights { get; set; }
        public string HighlightsAr { get; set; }
        public string TargetAudience { get; set; }
        public string TargetAudienceAr { get; set; }
        public bool IsActive { get; set; }
        public bool IsFeatured { get; set; }
        public int DisplayOrder { get; set; }
    }

    public class PagedLanguageCourseRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? LanguageCenterId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsFeatured { get; set; }
    }
}
