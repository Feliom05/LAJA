using System.ComponentModel.DataAnnotations;

namespace Laja.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "Förnamn:")]
        [Required(ErrorMessage = "Förnamnet krävs")]
        public string FirstName { get; set; }

        [Display(Name = "Efternamn:")]
        [Required(ErrorMessage = "Efternamet krävs")]
        public string LastName { get; set; }

        [Display(Name = "Email adress:")]
        [Required(ErrorMessage = "E-postadressen krävs")]
        [EmailAddress(ErrorMessage = "Ogiltig email adress")]
        public string Email { get; set; }

        [Display(Name = "Lösenord:")]
        public string PassWord { get; set; }

        [Display(Name = "Kursnamn:")]
        public int? CourseId { get; set; }

        [Required]
        public string Role { get; set; }
    }
}