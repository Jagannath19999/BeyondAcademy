using System;
using System.Collections.Generic;

namespace BeyondAcademy.Models
{
    public partial class Role
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
