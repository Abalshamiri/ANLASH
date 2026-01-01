using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ANLASH.LanguageCenters;

namespace ANLASH.LanguageCenters.Dto
{
    /// <summary>
    /// Language Center FAQ DTO
    /// </summary>
    [AutoMapFrom(typeof(LanguageCenterFAQ))]
    public class LanguageCenterFAQDto : FullAuditedEntityDto<long>
    {
        public long LanguageCenterId { get; set; }
        public string LanguageCenterName { get; set; }
        public string Question { get; set; }
        public string QuestionAr { get; set; }
        public string Answer { get; set; }
        public string AnswerAr { get; set; }
        public string Category { get; set; }
        public string CategoryAr { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; }
        public bool IsFeatured { get; set; }
        public int ViewCount { get; set; }
        public int HelpfulCount { get; set; }
        public int NotHelpfulCount { get; set; }
    }

    [AutoMapTo(typeof(LanguageCenterFAQ))]
    public class CreateLanguageCenterFAQDto
    {
        public long LanguageCenterId { get; set; }
        public string Question { get; set; }
        public string QuestionAr { get; set; }
        public string Answer { get; set; }
        public string AnswerAr { get; set; }
        public string Category { get; set; }
        public string CategoryAr { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; } = true;
        public bool IsFeatured { get; set; }
    }

    [AutoMapTo(typeof(LanguageCenterFAQ))]
    public class UpdateLanguageCenterFAQDto : EntityDto<long>
    {
        public long LanguageCenterId { get; set; }
        public string Question { get; set; }
        public string QuestionAr { get; set; }
        public string Answer { get; set; }
        public string AnswerAr { get; set; }
        public string Category { get; set; }
        public string CategoryAr { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsPublished { get; set; }
        public bool IsFeatured { get; set; }
    }

    public class PagedLanguageCenterFAQRequestDto : PagedAndSortedResultRequestDto
    {
        public string Keyword { get; set; }
        public long? LanguageCenterId { get; set; }
        public string Category { get; set; }
        public bool? IsPublished { get; set; }
        public bool? IsFeatured { get; set; }
    }

    /// <summary>
    /// FAQ Order DTO (for reordering)
    /// </summary>
    public class FAQOrderDto
    {
        public long Id { get; set; }
        public int DisplayOrder { get; set; }
    }
}
