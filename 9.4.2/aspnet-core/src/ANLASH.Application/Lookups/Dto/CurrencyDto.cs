using Abp.Application.Services.Dto;

namespace ANLASH.Lookups.Dto
{
    /// <summary>
    /// DTO للعملة - Currency DTO
    /// <para>يُستخدم لإرجاع بيانات العملة للواجهة الأمامية</para>
    /// <para>Used to return currency data to the frontend</para>
    /// </summary>
    public class CurrencyDto : EntityDto<int>
    {
        /// <summary>
        /// رمز العملة - Currency Code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// اسم العملة بالإنجليزية - Name in English
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// اسم العملة بالعربية - Name in Arabic
        /// </summary>
        public string NameAr { get; set; }

        /// <summary>
        /// رمز العملة - Symbol
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// هل العملة نشطة - Is Active
        /// </summary>
        public bool IsActive { get; set; }
    }
}
