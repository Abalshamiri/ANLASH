using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using ANLASH.Universities;

namespace ANLASH.Universities.Dto
{
    [AutoMapTo(typeof(UniversityFAQ))]
    public class CreateUniversityFAQDto
    {
        /// <summary>
        /// University ID | معرف الجامعة
        /// </summary>
        [Required]
        public long UniversityId { get; set; }

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
        public bool IsPublished { get; set; } = true;
    }
}
