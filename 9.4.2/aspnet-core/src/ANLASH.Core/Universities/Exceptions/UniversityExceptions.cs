using System;
using Abp.UI;

namespace ANLASH.Universities.Exceptions
{
    /// <summary>
    /// Exception thrown when a university is not found
    /// استثناء يُرمى عند عدم العثور على جامعة
    /// </summary>
    public class UniversityNotFoundException : UserFriendlyException
    {
        public UniversityNotFoundException()
            : base("University not found | الجامعة غير موجودة")
        {
        }

        public UniversityNotFoundException(long universityId)
            : base($"University with ID {universityId} not found | الجامعة برقم {universityId} غير موجودة")
        {
        }

        public UniversityNotFoundException(string slug)
            : base($"University with slug '{slug}' not found | الجامعة بالرابط '{slug}' غير موجودة")
        {
        }
    }

    /// <summary>
    /// Exception thrown when a duplicate slug is detected
    /// استثناء يُرمى عند اكتشاف رابط مكرر
    /// </summary>
    public class DuplicateSlugException : UserFriendlyException
    {
        public DuplicateSlugException(string slug)
            : base($"A university with slug '{slug}' already exists | جامعة بالرابط '{slug}' موجودة مسبقاً")
        {
        }
    }

    /// <summary>
    /// Exception thrown when invalid program level is provided
    /// استثناء يُرمى عند تقديم مستوى برنامج غير صحيح
    /// </summary>
    public class InvalidProgramLevelException : UserFriendlyException
    {
        public InvalidProgramLevelException(string level)
            : base($"Invalid program level: {level} | مستوى البرنامج غير صحيح: {level}")
        {
        }
    }

    /// <summary>
    /// Exception thrown when FAQ reordering fails
    /// استثناء يُرمى عند فشل إعادة ترتيب الأسئلة
    /// </summary>
    public class FAQReorderException : UserFriendlyException
    {
        public FAQReorderException()
            : base("Failed to reorder FAQs. Please try again | فشل إعادة ترتيب الأسئلة. الرجاء المحاولة مرة أخرى")
        {
        }

        public FAQReorderException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Exception thrown when content type already exists for a university
    /// استثناء يُرمى عند وجود نوع محتوى مسبقاً للجامعة
    /// </summary>
    public class DuplicateContentTypeException : UserFriendlyException
    {
        public DuplicateContentTypeException(string contentType, string universityName)
            : base($"Content type '{contentType}' already exists for {universityName} | نوع المحتوى '{contentType}' موجود مسبقاً لـ {universityName}")
        {
        }
    }
}
