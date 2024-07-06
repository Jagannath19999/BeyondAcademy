using BeyondAcademy.Controllers;
using BeyondAcademy.Models;

namespace BeyondAcademy.Interface
{
    public class RoleService : IRoleService
    {
        private readonly ApplicationDbContext _context;
        public RoleService(ApplicationDbContext context)
        {
            _context = context;
        }
        public string GetRoleNameForAcId(string acId)
        {
            var accountRole = _context.AccountRoles.FirstOrDefault(a => a.AcId.ToString() == acId && a.IsActive);
            if (accountRole == null) 
            {
                return null;
            }

            var role = _context.Roles.FirstOrDefault(a => a.RoleId == accountRole.RoleId && a.IsActive);
            if (role == null)
            {
                return null;
            }
            return role.RoleName;
        }
    }
}
