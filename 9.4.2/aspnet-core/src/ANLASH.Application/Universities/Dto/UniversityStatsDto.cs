namespace ANLASH.Universities.Dto
{
    /// <summary>
    /// Statistics DTO for university dashboard
    /// DTO إحصائيات للوحة التحكم
    /// </summary>
    public class UniversityStatsDto
    {
        /// <summary>
        /// University ID | معرف الجامعة
        /// </summary>
        public long UniversityId { get; set; }

        /// <summary>
        /// University name | اسم الجامعة
        /// </summary>
        public string UniversityName { get; set; }

        #region Program Stats | إحصائيات البرامج

        /// <summary>
        /// Total programs count | إجمالي البرامج
        /// </summary>
        public int TotalPrograms { get; set; }

        /// <summary>
        /// Active programs count | البرامج النشطة
        /// </summary>
        public int ActivePrograms { get; set; }

        /// <summary>
        /// Bachelor programs count | برامج البكالوريوس
        /// </summary>
        public int ProgramsByLevel_Bachelor { get; set; }

        /// <summary>
        /// Master programs count | برامج الماجستير
        /// </summary>
        public int ProgramsByLevel_Master { get; set; }

        /// <summary>
        /// PhD programs count | برامج الدكتوراه
        /// </summary>
        public int ProgramsByLevel_PhD { get; set; }

        #endregion

        #region Content Stats | إحصائيات المحتوى

        /// <summary>
        /// Total content sections | إجمالي المحتويات
        /// </summary>
        public int TotalContents { get; set; }

        /// <summary>
        /// Active content sections | المحتويات النشطة
        /// </summary>
        public int ActiveContents { get; set; }

        #endregion

        #region Media Stats | إحصائيات الوسائط

        /// <summary>
        /// Total gallery images | إجمالي صور المعرض
        /// </summary>
        public int TotalGalleryImages { get; set; }

        #endregion

        #region Engagement Stats | إحصائيات التفاعل (للمستقبل)

        /// <summary>
        /// Total views | إجمالي المشاهدات
        /// </summary>
        public int TotalViews { get; set; }

        /// <summary>
        /// Total inquiries | إجمالي الاستفسارات
        /// </summary>
        public int TotalInquiries { get; set; }

        #endregion
    }
}
