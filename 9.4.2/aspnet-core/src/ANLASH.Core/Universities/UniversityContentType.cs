using System.ComponentModel.DataAnnotations;

namespace ANLASH.Universities
{
    /// <summary>
    /// University Content Type Enumeration
    /// تعداد أنواع محتوى الجامعة
    /// </summary>
    public enum UniversityContentType
    {
        /// <summary>
        /// General overview of the university | نظرة عامة على الجامعة
        /// </summary>
        [Display(Name = "Overview", Description = "General overview of the university")]
        Overview = 1,
        
        /// <summary>
        /// About the university | عن الجامعة
        /// </summary>
        [Display(Name = "About", Description = "About the university")]
        About = 2,
        
        /// <summary>
        /// Admissions information | معلومات القبول
        /// </summary>
        [Display(Name = "Admissions", Description = "Admissions information and process")]
        Admissions = 3,
        
        /// <summary>
        /// Admission requirements | متطلبات القبول
        /// </summary>
        [Display(Name = "Requirements", Description = "Admission requirements and criteria")]
        Requirements = 4,
        
        /// <summary>
        /// Programs offered | البرامج المقدمة
        /// </summary>
        [Display(Name = "Programs", Description = "Academic programs offered")]
        Programs = 5,
        
        /// <summary>
        /// University rankings | تصنيفات الجامعة
        /// </summary>
        [Display(Name = "Rankings", Description = "University rankings and achievements")]
        Rankings = 6,
        
        /// <summary>
        /// Campus facilities | مرافق الحرم الجامعي
        /// </summary>
        [Display(Name = "Campus", Description = "Campus facilities and infrastructure")]
        Campus = 7,
        
        /// <summary>
        /// Available scholarships | المنح الدراسية المتاحة
        /// </summary>
        [Display(Name = "Scholarships", Description = "Available scholarships and financial aid")]
        Scholarships = 8,
        
        /// <summary>
        /// Frequently asked questions | الأسئلة الشائعة
        /// </summary>
        [Display(Name = "FAQ", Description = "Frequently asked questions")]
        FAQ = 9,
        
        /// <summary>
        /// Student life | الحياة الطلابية
        /// </summary>
        [Display(Name = "Student Life", Description = "Student life and activities")]
        StudentLife = 10,
        
        /// <summary>
        /// Research | البحث العلمي
        /// </summary>
        [Display(Name = "Research", Description = "Research activities and centers")]
        Research = 11,
        
        /// <summary>
        /// International students | الطلاب الدوليون
        /// </summary>
        [Display(Name = "International", Description = "Information for international students")]
        International = 12,
        
        /// <summary>
        /// Other content type | نوع محتوى آخر
        /// </summary>
        [Display(Name = "Other", Description = "Other content")]
        Other = 99
    }
}
