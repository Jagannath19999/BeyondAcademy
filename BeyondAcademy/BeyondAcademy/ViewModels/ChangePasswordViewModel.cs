using System.ComponentModel.DataAnnotations;

namespace BeyondAcademy.ViewModels
{
    public class ChangePasswordViewModel
    {
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New password and confirm password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}
