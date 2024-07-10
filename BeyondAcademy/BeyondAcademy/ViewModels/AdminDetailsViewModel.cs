using BeyondAcademy.Custom;
using System.ComponentModel.DataAnnotations;

namespace BeyondAcademy.ViewModels
{
    public class AdminDetailsViewModel
    {
        public Guid? RegdId { get; set; }
        public Guid? AcId { get; set; }
        public string? UserId { get; set; } = null!;

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? MobileNo { get; set; }

        public string? Email { get; set; }

        public string? DateOfBirth { get; set; }       
    }

}
