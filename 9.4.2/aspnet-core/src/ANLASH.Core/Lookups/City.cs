using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ANLASH.Lookups
{
    /// <summary>
    /// كيان المدينة - City Entity
    /// <para>يحتوي على معلومات المدن التابعة للدول</para>
    /// <para>Contains city information belonging to countries</para>
    /// </summary>
    [Table("Cities")]
    public class City : FullAuditedEntity<int>
    {
        /// <summary>
        /// اسم المدينة بالإنجليزية - City name in English
        /// <para>مثال: "Riyadh", "Jeddah", "Kuala Lumpur"</para>
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        /// <summary>
        /// اسم المدينة بالعربية - City name in Arabic
        /// <para>مثال: "الرياض", "جدة", "كوالالمبور"</para>
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string NameAr { get; set; }

        /// <summary>
        /// معرّف الدولة - Country ID (Foreign Key)
        /// </summary>
        [Required]
        public int CountryId { get; set; }

        /// <summary>
        /// الدولة التابعة لها - Related Country
        /// </summary>
        [ForeignKey(nameof(CountryId))]
        public virtual Country Country { get; set; }

        /// <summary>
        /// هل المدينة نشطة - Is city active
        /// </summary>
        public bool IsActive { get; set; } = true;

        // ✅ حقول التتبع موروثة من FullAuditedEntity:
        // - CreationTime, CreatorUserId
        // - LastModificationTime, LastModifierUserId
        // - DeletionTime, DeleterUserId, IsDeleted
    }
}
