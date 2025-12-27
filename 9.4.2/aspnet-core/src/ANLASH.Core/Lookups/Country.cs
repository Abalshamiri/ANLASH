using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ANLASH.Lookups
{
    /// <summary>
    /// كيان الدولة - Country Entity
    /// <para>يحتوي على معلومات الدول المستخدمة في النظام</para>
    /// <para>Contains country information used across the system</para>
    /// </summary>
    [Table("Countries")]
    public class Country : FullAuditedEntity<int>
    {
        /// <summary>
        /// رمز الدولة - Country Code (ISO 3166-1 alpha-2)
        /// <para>مثال: "SA", "US", "MY", "EG"</para>
        /// </summary>
        [Required]
        [MaxLength(3)]
        public string Code { get; set; }

        /// <summary>
        /// اسم الدولة بالإنجليزية - Country name in English
        /// <para>مثال: "Saudi Arabia", "United States"</para>
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// اسم الدولة بالعربية - Country name in Arabic
        /// <para>مثال: "المملكة العربية السعودية", "الولايات المتحدة"</para>
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string NameAr { get; set; }

        /// <summary>
        /// هل الدولة نشطة - Is country active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// المدن التابعة لهذه الدولة - Cities belonging to this country
        /// </summary>
        public virtual ICollection<City> Cities { get; set; }

        // ✅ حقول التتبع موروثة من FullAuditedEntity:
        // - CreationTime, CreatorUserId
        // - LastModificationTime, LastModifierUserId
        // - DeletionTime, DeleterUserId, IsDeleted
    }
}
