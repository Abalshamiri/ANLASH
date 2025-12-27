using Abp.Application.Services.Dto;

namespace ANLASH.Lookups.Dto
{
    /// <summary>
    /// DTO للدولة - Country DTO
    /// <para>يُستخدم لإرجاع بيانات الدولة للواجهة الأمامية</para>
    /// <para>Used to return country data to the frontend</para>
    /// </summary>
    public class CountryDto : EntityDto<int>
    {
        /// <summary>
        /// رمز الدولة - Country Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// اسم الدولة بالإنجليزية - Name in English
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// اسم الدولة بالعربية - Name in Arabic
        /// </summary>
        public string NameAr { get; set; }

        /// <summary>
        /// هل الدولة نشطة - Is Active
        /// </summary>
        public bool IsActive { get; set; }
    }
}
