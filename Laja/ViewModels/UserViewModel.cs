using System.ComponentModel.DataAnnotations;

namespace Laja.ViewModels
{
    public class UserViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Display(Name = "Email adress:")]
        [Required(ErrorMessage = "E-postadressen krävs")]
        [EmailAddress(ErrorMessage = "Ogiltig email adress")]
        public string Email { get; set; }

        public string PassWord { get; set; }

        public int? CourseId { get; set; }
    }
}