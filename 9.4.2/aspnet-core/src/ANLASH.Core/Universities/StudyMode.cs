using System.ComponentModel.DataAnnotations;

namespace ANLASH.Universities
{
    /// <summary>
    /// Study Mode Enumeration
    /// تعداد أنماط الدراسة
    /// </summary>
    public enum StudyMode
    {
        /// <summary>
        /// Full-time on-campus | دوام كامل في الحرم الجامعي
        /// </summary>
        [Display(Name = "Full-Time", Description = "Full-time on-campus")]
        FullTime = 1,
        
        /// <summary>
        /// Part-time program | دوام جزئي
        /// </summary>
        [Display(Name = "Part-Time", Description = "Part-time program")]
        PartTime = 2,
        
        /// <summary>
        /// Fully online | عبر الإنترنت بالكامل
        /// </summary>
        [Display(Name = "Online", Description = "Fully online")]
        Online = 3,
        
        /// <summary>
        /// Mixed online and on-campus | مختلط (حضوري وعن بعد)
        /// </summary>
        [Display(Name = "Hybrid", Description = "Mixed online and on-campus")]
        Hybrid = 4,
        
        /// <summary>
        /// Evening classes | دراسة مسائية
        /// </summary>
        [Display(Name = "Evening", Description = "Evening classes")]
        Evening = 5,
        
        /// <summary>
        /// Weekend classes | دراسة نهاية الأسبوع
        /// </summary>
        [Display(Name = "Weekend", Description = "Weekend classes")]
        Weekend = 6,
        
        /// <summary>
        /// Distance learning | تعليم عن بعد
        /// </summary>
        [Display(Name = "Distance", Description = "Distance learning")]
        Distance = 7
    }
}
