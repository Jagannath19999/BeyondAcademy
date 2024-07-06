using System;
using System.Collections.Generic;

namespace BeyondAcademy.Models
{
    public partial class AccountRole
    {
        public Guid Arid { get; set; }
        public Guid AcId { get; set; }
        public Guid RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
