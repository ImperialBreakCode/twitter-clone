using System.ComponentModel.DataAnnotations;

namespace TwitterUni.Models.User
{
    public class EditUserViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public string? ProfilePicBase64 { get; set; }

        [Display(Name = "Bio")]
        public string? Bio { get; set; }

        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Background Photo")]
        public IFormFile? BackgroundPhotoInput { get; set; }

        public string? BackgroundPhoto { get; set; }
        public string? ProfilePic { get; set; }
    }
}
