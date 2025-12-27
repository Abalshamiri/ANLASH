using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;

namespace ANLASH.Lookups
{
    /// <summary>
    /// كيان العملة - Currency Entity
    /// <para>يحتوي على معلومات العملات المستخدمة في النظام</para>
    /// <para>Contains currency information used across the system</para>
    /// </summary>
    [Table("Currencies")]
    public class Currency : FullAuditedEntity<int>
    {
        /// <summary>
        /// رمز العملة - Currency Code
        /// <para>مثال: "SAR", "USD", "EUR", "MYR"</para>
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        /// <summary>
        /// اسم العملة بالإنجليزية - Currency name in English
        /// <para>مثال: "Saudi Riyal", "US Dollar"</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        /// <summary>
        /// اسم العملة بالعربية - Currency name in Arabic
        /// <para>مثال: "ريال سعودي", "دولار أمريكي"</para>
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string NameAr { get; set; }

        /// <summary>
        /// رمز العملة - Currency Symbol
        /// <para>مثال: "﷼", "$", "€"</para>
        /// </summary>
        [MaxLength(10)]
        public string Symbol { get; set; }

        /// <summary>
        /// هل العملة نشطة - Is currency active
        /// </summary>
        public bool IsActive { get; set; } = true;

        // ✅ حقول التتبع موروثة من FullAuditedEntity:
        // - CreationTime (DateTime) - تاريخ الإنشاء
        // - CreatorUserId (long?) - من أنشأ السجل
        // - LastModificationTime (DateTime?) - تاريخ آخر تعديل
        // - LastModifierUserId (long?) - من عدّل السجل
        // - DeletionTime (DateTime?) - تاريخ الحذف
        // - DeleterUserId (long?) - من حذف السجل
        // - IsDeleted (bool) - محذوف؟
    }
}
