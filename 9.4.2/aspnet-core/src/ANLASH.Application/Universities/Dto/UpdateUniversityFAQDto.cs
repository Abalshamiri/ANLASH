using System.ComponentModel.DataAnnotations;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ANLASH.Universities;

namespace ANLASH.Universities.Dto
{
    [AutoMapTo(typeof(UniversityFAQ))]
    public class UpdateUniversityFAQDto : EntityDto<long>
    {
        /// <summary>
        /// Question in English | السؤال بالإنجليزية
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Question { get; set; }

        /// <summary>
        /// Question in Arabic | السؤال بالعربية
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string QuestionAr { get; set; }

        /// <summary>
        /// Answer in English | الإجابة بالإنجليزية
        /// </summary>
        [Required]
        [MaxLength(2000)]
        public string Answer { get; set; }

        /// <summary>
        /// Answer in Arabic | الإجابة بالعربية
        /// </summary>
        [Required]
        [MaxLength(2000)]
        public string AnswerAr { get; set; }

        /// <summary>
        /// Display order | ترتيب العرض
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Is published | هل منشور
        /// </summary>
        public bool IsPublished { get; set; }
    }
}
