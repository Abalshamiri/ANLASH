using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ANLASH.Universities;

namespace ANLASH.Universities.Dto
{
    [AutoMapFrom(typeof(UniversityFAQ))]
    public class UniversityFAQDto : EntityDto<long>
    {
        /// <summary>
        /// University ID | معرف الجامعة
        /// </summary>
        public long UniversityId { get; set; }

        /// <summary>
        /// Question in English | السؤال بالإنجليزية
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Question in Arabic | السؤال بالعربية
        /// </summary>
        public string QuestionAr { get; set; }

        /// <summary>
        /// Answer in English | الإجابة بالإنجليزية
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Answer in Arabic | الإجابة بالعربية
        /// </summary>
        public string AnswerAr { get; set; }

        /// <summary>
        /// Display order | ترتيب العرض
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Is published | هل منشور
        /// </summary>
        public bool IsPublished { get; set; }

        // Calculated fields
        public string UniversityName { get; set; }
        public string UniversityNameAr { get; set; }
    }
}
