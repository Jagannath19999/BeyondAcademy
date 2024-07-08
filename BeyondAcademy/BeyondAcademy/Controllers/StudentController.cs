using BeyondAcademy.Interface;
using BeyondAcademy.Models;
using BeyondAcademy.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeyondAcademy.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoleService _roleService;
        public StudentController(ILogger<HomeController> logger, ApplicationDbContext context, IRoleService roleService)
        {
            _context = context;
            _roleService = roleService;
        }
        public IActionResult StudentDashboard()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(StudentRegistrationViewModel data)
        {
            if (ModelState.IsValid)
            {
                var existStudent = _context.Registrations.Where(r => r.Email == data.Email).FirstOrDefault();

                if (existStudent != null)
                {
                    ViewData["ErrorMessage"] = "Student already exists";
                    return View();
                }

                var newStudent = new Registration
                {
                    RegdId = Guid.NewGuid(),
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    MobileNo = data.MobileNo,
                    Email = data.Email,
                    DateOfBirth = data.DateOfBirth.ToString(),
                    IsActive = true,
                    CreatedBy = "Student",
                    CreatedOn = DateTime.Now,
                };

                var newAccount = new Account
                {
                    AcId = Guid.NewGuid(),
                    RegdId = newStudent.RegdId,
                    Name = data.FirstName,
                    Email= data.Email,
                    UserId = $"{data.Email.Split('@')[0]}{data.MobileNo.Substring(data.MobileNo.Length - 4)}",
                    Password = _roleService.HashPassword(data.NewPassword),
                    IsActive = true,
                    CreatedBy = "Student",
                    CreatedOn = DateTime.Now,
                };

                _context.Add(newAccount);
                await _context.SaveChangesAsync();

                var roleId = _context.Roles.FirstOrDefault( r => r.RoleName == "Student").RoleId;
                if (roleId != null)
                {
                    var accountRole = new AccountRole
                    {
                        Arid = Guid.NewGuid(),
                        AcId = newAccount.AcId,
                        RoleId = roleId,
                        IsActive = true,
                        CreatedBy = "Student",
                        CreatedOn = DateTime.Now,
                    };
                    _context.Add(accountRole);
                    await _context.SaveChangesAsync();
                }

                _context.Add(newStudent);
                await _context.SaveChangesAsync();

                ViewData["SuccessMessage"] = "Student registered successfully";
                return View();
            }
            ViewData["ErrorMessage"] = "Error occured";
            return View();
        }
    }
}
