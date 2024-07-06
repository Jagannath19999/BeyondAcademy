using BeyondAcademy.Interface;
using BeyondAcademy.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net.Sockets;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;

namespace BeyondAcademy.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IRoleService _roleService;
        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IRoleService roleService)
        {
            _logger = logger;
            _context = context;
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string userId, string password)
        {
            var user = _context.Accounts.FirstOrDefault(x => x.UserId == userId && x.Password == HashPassword(password) && x.IsActive);
            if (user != null)
            {
                var regId = user.RegdId.ToString();
                var acId = user.AcId.ToString();
                HttpContext.Session.SetString("RegdId", regId);
                HttpContext.Session.SetString("AcId", acId);

                var roleName = _roleService.GetRoleNameForAcId(user.AcId.ToString());
                if (roleName.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("AdminDashboard", "Admin");
                }
                else if (roleName.Equals("Teacher", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("TeacherDashboard", "Teacher");
                }
                else if (roleName.Equals("Student", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToAction("StudentDashboard", "Student");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Unauthorized Role.");
                    return View();
                }

            }
            ModelState.AddModelError(string.Empty, "Incorrect username or password entered.");
            return View();
        }

        [HttpPost]
        public IActionResult Logout(string userId, string password)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Home");
        }


        private string HashPassword(string password)
        {
            using (var ms = SHA256.Create())
            {
                var hashedBytes = ms.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hashPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hashPassword;
            }
        }
        
    }
}
