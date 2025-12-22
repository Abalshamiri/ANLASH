using System.ComponentModel.DataAnnotations;

namespace ANLASH.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}