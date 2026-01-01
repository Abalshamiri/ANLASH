using System.ComponentModel;

namespace ANLASH.LanguageCenters
{
    /// <summary>
    /// نوع الدورة اللغوية - Language Course Type
    /// <para>Categorizes language courses by their focus and target audience</para>
    /// </summary>
    public enum CourseType
    {
        /// <summary>
        /// دورة لغة عامة - General language course
        /// <para>For everyday communication and general language skills</para>
        /// </summary>
        [Description("General Language")]
        General = 0,

        /// <summary>
        /// دورة لغة أكاديمية - Academic language course
        /// <para>Preparation for university studies and academic contexts</para>
        /// </summary>
        [Description("Academic Language")]
        Academic = 1,

        /// <summary>
        /// دورة لغة الأعمال - Business language course
        /// <para>Focused on business communication and professional contexts</para>
        /// </summary>
        [Description("Business Language")]
        Business = 2,

        /// <summary>
        /// دورة تحضير للاختبارات - Test preparation course
        /// <para>IELTS, TOEFL, Cambridge, etc.</para>
        /// </summary>
        [Description("Test Preparation")]
        TestPreparation = 3,

        /// <summary>
        /// دورة مكثفة - Intensive course
        /// <para>Accelerated learning with more hours per week</para>
        /// </summary>
        [Description("Intensive")]
        Intensive = 4,

        /// <summary>
        /// دورة صيفية - Summer course
        /// <para>Seasonal programs during summer vacation</para>
        /// </summary>
        [Description("Summer Program")]
        Summer = 5,

        /// <summary>
        /// دورة للمبتدئين - Beginner course
        /// <para>Specifically designed for absolute beginners</para>
        /// </summary>
        [Description("Beginner")]
        Beginner = 6,

        /// <summary>
        /// دورة متخصصة - Specialized course
        /// <para>Medical, Legal, Technical, etc.</para>
        /// </summary>
        [Description("Specialized")]
        Specialized = 7
    }
}
