using Abp.Application.Services.Dto;

namespace ANLASH.Lookups.Dto
{
    /// <summary>
    /// DTO للمدينة - City DTO
    /// <para>يُستخدم لإرجاع بيانات المدينة للواجهة الأمامية</para>
    /// <para>Used to return city data to the frontend</para>
    /// </summary>
    public class CityDto : EntityDto<int>
    {
        /// <summary>
        /// اسم المدينة بالإنجليزية - Name in English
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// اسم المدينة بالعربية - Name in Arabic
        /// </summary>
        public string NameAr { get; set; }

        /// <summary>
        /// معرّف الدولة - Country ID
        /// </summary>
        public int CountryId { get; set; }

        /// <summary>
        /// اسم الدولة بالإنجليزية - Country Name in English
        /// </summary>
        public string CountryName { get; set; }

        /// <summary>
        /// اسم الدولة بالعربية - Country Name in Arabic
        /// </summary>
        public string CountryNameAr { get; set; }

        /// <summary>
        /// هل المدينة نشطة - Is Active
        /// </summary>
        public bool IsActive { get; set; }
    }
}
