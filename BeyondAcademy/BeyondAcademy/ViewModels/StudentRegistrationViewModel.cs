using BeyondAcademy.Custom;
using System.ComponentModel.DataAnnotations;

namespace BeyondAcademy.ViewModels
{
    public class StudentRegistrationViewModel
    {
        public Guid? RegdId { get; set; }

        [Required(ErrorMessage = "First Name can't be empty")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "First Name should be character only")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name can't be empty")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Last Name should be character only")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Mobile Number can't be empty")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile Number should be 10 digits")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Email address can't be empty")]
        [EmailAddress(ErrorMessage = "Invalid Email Address format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter your date of birth")]
        [DataType(DataType.Date)]
        [PastDate(ErrorMessage = "Date of Birth can't be today or a future date")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "New password can't be empty")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new password can't be empty")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "New password and confirm password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}
