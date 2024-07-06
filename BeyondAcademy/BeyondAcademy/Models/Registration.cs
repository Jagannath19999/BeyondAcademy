using System;
using System.Collections.Generic;

namespace BeyondAcademy.Models
{
    public partial class Registration
    {
        public Guid RegdId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string MobileNo { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string DateOfBirth { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
