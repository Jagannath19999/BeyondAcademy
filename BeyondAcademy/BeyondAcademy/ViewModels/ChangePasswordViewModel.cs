using System.ComponentModel.DataAnnotations;

namespace BeyondAcademy.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Current password can't be empty")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password can't be empty")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm password can't be empty")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New password and confirm password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}
