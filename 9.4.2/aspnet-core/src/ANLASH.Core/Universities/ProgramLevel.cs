using System.ComponentModel.DataAnnotations;

namespace ANLASH.Universities
{
    /// <summary>
    /// Program Level Enumeration
    /// تعداد المستويات الدراسية للبرامج
    /// </summary>
    public enum ProgramLevel
    {
        /// <summary>
        /// Diploma program | برنامج دبلوم
        /// </summary>
        [Display(Name = "Diploma", Description = "Diploma program")]
        Diploma = 1,
        
        /// <summary>
        /// Associate degree | درجة مشارك
        /// </summary>
        [Display(Name = "Associate", Description = "Associate degree")]
        Associate = 2,
        
        /// <summary>
        /// Bachelor's degree | درجة بكالوريوس
        /// </summary>
        [Display(Name = "Bachelor", Description = "Bachelor's degree")]
        Bachelor = 3,
        
        /// <summary>
        /// Post-graduate diploma | دبلوم دراسات عليا
        /// </summary>
        [Display(Name = "Post-Graduate Diploma", Description = "Post-graduate diploma")]
        PostGraduateDiploma = 4,
        
        /// <summary>
        /// Master's degree | درجة ماجستير
        /// </summary>
        [Display(Name = "Master", Description = "Master's degree")]
        Master = 5,
        
        /// <summary>
        /// Doctorate degree | درجة دكتوراه
        /// </summary>
        [Display(Name = "PhD", Description = "Doctorate degree")]
        PhD = 6,
        
        /// <summary>
        /// Professional certificate | شهادة مهنية
        /// </summary>
        [Display(Name = "Certificate", Description = "Professional certificate")]
        Certificate = 7,
        
        /// <summary>
        /// Other program type | نوع برنامج آخر
        /// </summary>
        [Display(Name = "Other", Description = "Other program type")]
        Other = 99
    }
}
