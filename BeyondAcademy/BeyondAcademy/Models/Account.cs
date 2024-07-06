using System;
using System.Collections.Generic;

namespace BeyondAcademy.Models
{
    public partial class Account
    {
        public Guid AcId { get; set; }
        public Guid? RegdId { get; set; }
        public string Name { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
