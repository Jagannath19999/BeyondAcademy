using BeyondAcademy.Interface;
using BeyondAcademy.Models;
using BeyondAcademy.ViewModels;
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
            var user = _context.Accounts.FirstOrDefault(x => (x.UserId == userId || x.Email == userId) && x.Password == HashPassword(password));
            if (user != null && !user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Your account is disabled, please contact administrator");
                return View();
            }
            if (user != null && user.IsActive)
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
            ModelState.AddModelError(string.Empty, "Incorrect username or password entered");
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

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string userIdorEmail, string newPassword, string confirmPassword)
        {
            if (string.IsNullOrEmpty(userIdorEmail) || string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                ViewData["ErrorMessage"] = "Please fill the all fields";
                return View();
            }

            if (newPassword != confirmPassword)
            {
                ViewData["ErrorMessage"] = "Password doesn't match";
            }

            var sameOldPassword = _context.Accounts.FirstOrDefault(u => (u.UserId == userIdorEmail || u.Email == userIdorEmail) && u.Password == HashPassword(newPassword));
            if (sameOldPassword != null) 
            {
                ViewData["ErrorMessage"] = "New password same as old password, please use a different password";
                return View();
            }

            var user = _context.Accounts.FirstOrDefault(u => (u.Email == userIdorEmail || u.UserId == userIdorEmail) && u.IsActive);

            if (user == null)
            {
                ViewData["ErrorMessage"] = "No account found with this UserID or Email";
                return View();
            }

            user.Password = HashPassword(newPassword);
            _context.Update(user);
            await _context.SaveChangesAsync();

            ViewData["SuccessMessage"] = "Password reset successfully";
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel data)
        {
            if (!ModelState.IsValid)
            {
                return View(data);
            }

            var accountId = HttpContext.Session.GetString("AcId");
            if (string.IsNullOrEmpty(accountId))
            {
                return RedirectToAction(nameof(Login));
            }

            var account = _context.Accounts.FirstOrDefault(a => a.AcId.ToString() == accountId);

            if (account == null)
            {
                ViewData["ErrorMessage"] = "Invalid User";
                return View(data);
            }

            if (data.CurrentPassword == data.NewPassword)
            {
                ViewData["ErrorMessage"] = "New password can't be same as current password";
                return View(data);
            }

            var checkCurrentPassword = _context.Accounts.FirstOrDefault(a => a.AcId.ToString() == accountId && a.Password == HashPassword(data.CurrentPassword));
            if (checkCurrentPassword == null)
            {
                ViewData["ErrorMessage"] = "Incorrect current password";
                return View(data);
            }

            if (data.NewPassword != data.ConfirmPassword)
            {
                ViewData["ErrorMessage"] = "New password and confirm password don't match";
                return View(data);
            }

            account.Password = HashPassword(data.NewPassword);
            account.ModifiedBy = account.Name;
            account.Modified = DateTime.Now;

            _context.Update(account);
            await _context.SaveChangesAsync();

            ViewData["SuccessMessage"] = "Password changed successfully";
            return View();
        }
    }
}
