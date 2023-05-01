using System.ComponentModel.DataAnnotations;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Areas.Account.Models.Settings
{
    public class SettingsViewModel
    {
        public UserData? User { get; set; }

        public bool DeleteFormHasErrors { get; set; } = false;

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
