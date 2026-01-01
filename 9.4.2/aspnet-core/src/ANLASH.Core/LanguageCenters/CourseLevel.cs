using System.ComponentModel;

namespace ANLASH.LanguageCenters
{
    /// <summary>
    /// مستوى الدورة اللغوية - Language Course Level
    /// <para>Standardized proficiency levels based on CEFR (Common European Framework of Reference)</para>
    /// </summary>
    public enum CourseLevel
    {
        /// <summary>
        /// مبتدئ - Beginner (A1)
        /// <para>CEFR A1 - Can understand and use familiar everyday expressions</para>
        /// </summary>
        [Description("Beginner (A1)")]
        Beginner = 0,

        /// <summary>
        /// ابتدائي - Elementary (A2)
        /// <para>CEFR A2 - Can communicate in simple and routine tasks</para>
        /// </summary>
        [Description("Elementary (A2)")]
        Elementary = 1,

        /// <summary>
        /// ما قبل المتوسط - Pre-Intermediate (A2+)
        /// <para>Bridge between A2 and B1</para>
        /// </summary>
        [Description("Pre-Intermediate (A2+)")]
        PreIntermediate = 2,

        /// <summary>
        /// متوسط - Intermediate (B1)
        /// <para>CEFR B1 - Can deal with most situations while traveling</para>
        /// </summary>
        [Description("Intermediate (B1)")]
        Intermediate = 3,

        /// <summary>
        /// فوق المتوسط - Upper-Intermediate (B2)
        /// <para>CEFR B2 - Can interact with native speakers with fluency</para>
        /// </summary>
        [Description("Upper-Intermediate (B2)")]
        UpperIntermediate = 4,

        /// <summary>
        /// متقدم - Advanced (C1)
        /// <para>CEFR C1 - Can use language flexibly and effectively</para>
        /// </summary>
        [Description("Advanced (C1)")]
        Advanced = 5,

        /// <summary>
        /// إتقان - Proficiency (C2)
        /// <para>CEFR C2 - Can understand virtually everything with ease</para>
        /// </summary>
        [Description("Proficiency (C2)")]
        Proficiency = 6,

        /// <summary>
        /// جميع المستويات - All Levels
        /// <para>Course suitable for all proficiency levels</para>
        /// </summary>
        [Description("All Levels")]
        AllLevels = 99
    }
}
