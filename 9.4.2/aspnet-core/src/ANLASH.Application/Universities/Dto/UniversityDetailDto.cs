using System;
using System.Collections.Generic;
using Abp.AutoMapper;
using ANLASH.Universities;

namespace ANLASH.Universities.Dto
{
    /// <summary>
    /// Complete DTO for university detail page
    /// Includes all related data (programs, contents, media)
    /// DTO كامل لصفحة تفاصيل الجامعة
    /// </summary>
    [AutoMapFrom(typeof(University))]
    public class UniversityDetailDto : UniversityListDto
    {
        #region Rich Content | المحتوى الغني

        /// <summary>
        /// About text (English) | نبذة عن الجامعة بالإنجليزية
        /// </summary>
        public string AboutText { get; set; }

        /// <summary>
        /// About text (Arabic) | نبذة عن الجامعة بالعربية
        /// </summary>
        public string AboutTextAr { get; set; }

        #endregion

        #region Media | الوسائط

        /// <summary>
        /// Logo blob ID | معرف شعار الجامعة
        /// </summary>
        public Guid? LogoBlobId { get; set; }

        /// <summary>
        /// Cover image blob ID | معرف صورة الغلاف
        /// </summary>
        public Guid? CoverImageBlobId { get; set; }

        /// <summary>
        /// Cover image URL | رابط صورة الغلاف
        /// </summary>
        public string CoverImageUrl { get; set; }

        #endregion

        #region SEO

        /// <summary>
        /// Meta description (English) | وصف ميتا بالإنجليزية
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Meta description (Arabic) | وصف ميتا بالعربية
        /// </summary>
        public string MetaDescriptionAr { get; set; }

        #endregion

        #region Additional Metadata | معلومات إضافية

        /// <summary>
        /// Offer letter fee | رسوم خطاب القبول
        /// </summary>
        public string OfferLetterFee { get; set; }

        /// <summary>
        /// Intake months | أشهر القبول
        /// </summary>
        public List<string> IntakeMonths { get; set; }

        #endregion

        #region Related Data | البيانات المرتبطة

        /// <summary>
        /// University programs | برامج الجامعة
        /// </summary>
        public List<UniversityProgramDto> Programs { get; set; }

        /// <summary>
        /// University content sections | محتويات الجامعة
        /// </summary>
        public List<UniversityContentDto> Contents { get; set; }

        /// <summary>
        /// University FAQs | الأسئلة الشائعة للجامعة
        /// </summary>
        public List<UniversityFAQDto> FAQs { get; set; }

        /// <summary>
        /// Gallery images | صور المعرض
        /// </summary>
        public List<MediaItemDto> Gallery { get; set; }

        #endregion

        #region Statistics | الإحصائيات

        /// <summary>
        /// Total programs count | إجمالي البرامج
        /// </summary>
        public int TotalPrograms { get; set; }

        /// <summary>
        /// Active programs count | البرامج النشطة
        /// </summary>
        public int ActivePrograms { get; set; }

        #endregion

        #region Audit Info | معلومات التدقيق

        /// <summary>
        /// Creation time | تاريخ الإنشاء
        /// </summary>
        public DateTime CreationTime { get; set; }

        /// <summary>
        /// Last modification time | تاريخ آخر تعديل
        /// </summary>
        public DateTime? LastModificationTime { get; set; }

        #endregion
    }
}
