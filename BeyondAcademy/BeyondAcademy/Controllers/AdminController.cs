using BeyondAcademy.Interface;
using BeyondAcademy.Models;
using BeyondAcademy.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace BeyondAcademy.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRoleService _roleService;
        public AdminController(ApplicationDbContext context, IRoleService roleService)
        {
            _context = context;
            _roleService = roleService;
        }

        #region Admin Region
        public IActionResult AdminDashboard()
        {
            return View();
        }

        public IActionResult MyProfile()
        {
            var accountId = HttpContext.Session.GetString("AcId");
            var regdId = _context.Accounts.FirstOrDefault(a => a.AcId.ToString() == accountId)?.RegdId;
            var roleName = _roleService.GetRoleNameForAcId(accountId);

            if (roleName == null)
            {
                ModelState.AddModelError(string.Empty, "No role is assigned to you");
                return RedirectToAction("Login", "Home");
            }

            if (roleName == "Student" || roleName == "Teacher")
            {
                ModelState.AddModelError(string.Empty, "You are authorized");
                return RedirectToAction("Login", "Home");
            }

            var adminDetails = (from reg in _context.Registrations
                                join acc in _context.Accounts on reg.RegdId equals acc.RegdId
                                where reg.RegdId == regdId
                                select new AdminDetailsViewModel
                                {
                                    RegdId = reg.RegdId,
                                    FirstName = reg.FirstName,
                                    LastName = reg.LastName,
                                    MobileNo = reg.MobileNo,
                                    Email = reg.Email,
                                    DateOfBirth = reg.DateOfBirth,
                                    AcId = acc.AcId,
                                    UserId = acc.UserId,
                                }).FirstOrDefault();
            if (adminDetails == null)
            {
                ViewData["ErrorMessage"] = "Admin details not found";
                return View();
            }

            return View(adminDetails);
        }

        public IActionResult UpdateProfile(Guid AcId, Guid RegdId)
        {
            var accountId = HttpContext.Session.GetString("AcId");
            var regdId = _context.Accounts.FirstOrDefault(a => a.AcId.ToString() == accountId)?.RegdId;
            var roleName = _roleService.GetRoleNameForAcId(accountId);

            if (roleName == null)
            {
                ModelState.AddModelError(string.Empty, "No role is assigned to you");
                return RedirectToAction("Login", "Home");
            }

            if (roleName == "Student" || roleName == "Teacher")
            {
                ModelState.AddModelError(string.Empty, "You are not authorized");
                return RedirectToAction("Login", "Home");
            }

            var adminDetails = (from reg in _context.Registrations
                                join acc in _context.Accounts on reg.RegdId equals acc.RegdId
                                where reg.RegdId == RegdId
                                select new AdminDetailsViewModel
                                {
                                    RegdId = reg.RegdId,
                                    FirstName = reg.FirstName,
                                    LastName = reg.LastName,
                                    MobileNo = reg.MobileNo,
                                    Email = reg.Email,
                                    DateOfBirth = reg.DateOfBirth,
                                    AcId = acc.AcId,
                                    UserId = acc.UserId,
                                }).FirstOrDefault();

            if (adminDetails == null)
            {
                ModelState.AddModelError(string.Empty, "Admin details not found");
                return RedirectToAction("Login", "Home");
            }
            if (TempData.Values.Count != 0)
            {
                ViewData["SuccessMessage"] = TempData["SuccessMessage"];
                ViewData["ErrorMessage"] = TempData["ErrorMessage"];
                return View(adminDetails);
            }
            return View(adminDetails);
        }

        [HttpPost]
        public IActionResult UpdateProfile(AdminDetailsViewModel model)
        {
            bool isValid = true;

            if (model.FirstName.Any(char.IsDigit) || model.LastName.Any(char.IsDigit))
            {
                ModelState.AddModelError("", "First Name and Last Name cannot contain numbers");
                isValid = false;
            }

            if (!Regex.IsMatch(model.MobileNo, @"^\d{10}$"))
            {
                ModelState.AddModelError("MobileNo", "Mobile Number must be exactly 10 digits");
                isValid = false;
            }

            if (string.IsNullOrEmpty(model.MobileNo))
            {
                ModelState.AddModelError("MobileNo", "Mobile Number can't be alphabet");
                isValid = false;
            }

            try
            {
                var mailAddress = new MailAddress(model.Email);
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Email", "Invalid Email Address");
                isValid = false;
            }

            DateTime dateOfBirth;
            if (!DateTime.TryParse(model.DateOfBirth.ToString(), out dateOfBirth))
            {
                ModelState.AddModelError("DateOfBirth", "Invalid Date of Birth format");
                isValid = false;
            }
            else if (dateOfBirth > DateTime.Today)
            {
                ModelState.AddModelError("DateOfBirth", "Date of Birth cannot be in the future");
                isValid = false;
            }

            if (!isValid)
            {
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["ErrorMessage"] = string.Join(" | ", errorMessages);
                return RedirectToAction("UpdateProfile", new { AcId = model.AcId, RegdId = model.RegdId });
            }

            var registration = _context.Registrations.FirstOrDefault(r => r.RegdId == model.RegdId);
            var account = _context.Accounts.FirstOrDefault(a => a.AcId == model.AcId);

            if (registration != null && account != null)
            {
                registration.FirstName = model.FirstName;
                registration.LastName = model.LastName;
                registration.MobileNo = model.MobileNo;
                registration.Email = model.Email;
                registration.DateOfBirth = model.DateOfBirth;

                account.UserId = model.UserId;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("UpdateProfile", new { AcId = model.AcId, RegdId = model.RegdId });
            }

            TempData["ErrorMessage"] = "Registration or account not found";
            return View(model);
        }
        #endregion

        #region Manage Teacher Region
        public IActionResult TeacherIndex()
        {
            var teachers = _context.Registrations.Where(r => r.CreatedBy == "Admin")
                           .Select(reg => new TeacherViewModel
                           {
                               RegdId = reg.RegdId,
                               FirstName = reg.FirstName,
                               LastName = reg.LastName,
                               MobileNo = reg.MobileNo,
                               Email = reg.Email,
                               DateOfBirth = reg.DateOfBirth,
                               IsActive = reg.IsActive
                           }).ToList();

            return View(teachers);
        }

        public IActionResult AddTeacher()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTeacher(TeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                DateTime dob = DateTime.Parse(model.DateOfBirth);
                if (dob > DateTime.Now)
                {
                    TempData["ErrorMessage"] = "Date of birth can not be inserted after today";
                    return View(model);
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    TempData["ErrorMessage"] = "New password and the confirm password can not be different";
                    return View(model);
                }

                var existTeacherMobileNo = _context.Registrations.Where(r => r.MobileNo == model.MobileNo).FirstOrDefault();
                if (existTeacherMobileNo != null)
                {
                    TempData["ErrorMessage"] = "Mobile number is already exists.";
                    return View(model);
                }

                var existTeacherEmail = _context.Registrations.Where(r => r.Email == model.Email).FirstOrDefault();
                if (existTeacherEmail != null)
                {
                    TempData["ErrorMessage"] = "Email is already exists.";
                    return View(model);
                }

                var reg = new Registration
                {
                    RegdId = Guid.NewGuid(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    MobileNo = model.MobileNo,
                    Email = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    IsActive = true,
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.Now,
                };

                var acc = new Account
                {
                    AcId = Guid.NewGuid(),
                    RegdId = reg.RegdId,
                    Name = model.FirstName,
                    Email = model.Email,
                    UserId = $"{model.Email.Split('@')[0]}{model.MobileNo.Substring(model.MobileNo.Length - 4)}",
                    Password = _roleService.HashPassword(model.NewPassword),
                    IsActive = true,
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.Now,
                };

                var roleId = _context.Roles.FirstOrDefault(r => r.RoleName == "Teacher").RoleId;
                var accRole = new AccountRole
                {
                    Arid = Guid.NewGuid(),
                    AcId = acc.AcId,
                    RoleId = roleId.Value,
                    IsActive = true,
                    CreatedBy = "Admin",
                    CreatedOn = DateTime.Now,
                };

                _context.Registrations.Add(reg);
                _context.Accounts.Add(acc);
                _context.AccountRoles.Add(accRole);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Teacher created successfully!";
                return View();
            }

            TempData["ErrorMessage"] = "Error adding teacher. Please check the details and try again.";
            return View(model);
        }

        public IActionResult UpdateTeacher(Guid id)
        {
            var disableTeacher = _context.Registrations.Where(r => r.RegdId == id && r.IsActive).FirstOrDefault();
            if (disableTeacher == null)
            {
                TempData["ErrorMessage"] = "Please enable this teacher account first";
                return RedirectToAction("TeacherIndex", TempData);
            }
            var teacher = (from reg in _context.Registrations
                           join acc in _context.Accounts on reg.RegdId equals acc.RegdId
                           where reg.RegdId == id
                           select new TeacherViewModel
                           {
                               RegdId = reg.RegdId,
                               FirstName = reg.FirstName,
                               LastName = reg.LastName,
                               MobileNo = reg.MobileNo,
                               Email = reg.Email,
                               //DateOfBirth = DateTime.Parse(reg.DateOfBirth),
                               DateOfBirth = reg.DateOfBirth,
                           }).FirstOrDefault();

            if (teacher == null)
            {
                TempData["ErrorMessage"] = "Teacher not found.";
                return RedirectToAction("TeacherIndex");
            }

            return View(teacher);
        }

        [HttpPost]
        public IActionResult UpdateTeacher(TeacherViewModel model)
        {
            if (ModelState.IsValid)
            {
                DateTime dob = DateTime.Parse(model.DateOfBirth);
                if (dob > DateTime.Now)
                {
                    TempData["ErrorMessage"] = "Date of birth can not be inserted after today";
                    return View(model);
                }
                if (model.NewPassword != model.ConfirmPassword)
                {
                    TempData["ErrorMessage"] = "New password and the confirm password can not be different";
                    return View(model);
                }

                var reg = _context.Registrations.FirstOrDefault(r => r.RegdId == model.RegdId && r.IsActive);
                var acc = _context.Accounts.FirstOrDefault(a => a.RegdId == model.RegdId && reg.IsActive);

                var checkDuplicateMobileNoEntry = _context.Registrations.Where(r => r.MobileNo == model.MobileNo && r.IsActive).FirstOrDefault();
                if (checkDuplicateMobileNoEntry != null && (model.MobileNo == checkDuplicateMobileNoEntry.MobileNo) && (model.MobileNo != reg.MobileNo))
                {
                    TempData["ErrorMessage"] = "Mobile number already exists";
                    return View(model);
                }
                var checkDuplicateEmailEntry = _context.Registrations.Where(r => r.Email == model.Email && r.IsActive && (model.Email != reg.Email)).FirstOrDefault();
                if (checkDuplicateEmailEntry != null && (model.Email == checkDuplicateEmailEntry.Email))
                {
                    TempData["ErrorMessage"] = "Email address already exists";
                    return View(model);
                }

                if (reg != null && acc != null)
                {
                    reg.FirstName = model.FirstName;
                    reg.LastName = model.LastName;
                    reg.MobileNo = model.MobileNo;
                    reg.Email = model.Email;
                    reg.DateOfBirth = model.DateOfBirth;

                    acc.Email = model.Email;

                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Teacher updated successfully!";
                    return View();
                }

                TempData["ErrorMessage"] = "Teacher not found.";
                return View(model);
            }
            TempData["ErrorMessage"] = "Provide the valid teacher data";
            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteTeacher(Guid id)
        {
            var reg = _context.Registrations.FirstOrDefault(r => r.RegdId == id);
            var acc = _context.Accounts.FirstOrDefault(a => a.RegdId == id);
            var roleId = _context.Roles.FirstOrDefault(r => r.RoleName == "Teacher")?.RoleId;
            var accRole = _context.AccountRoles.FirstOrDefault(ar => ar.AcId == acc.AcId && ar.RoleId == roleId);

            if (reg != null && acc != null && accRole != null)
            {
                reg.IsActive = false;
                acc.IsActive = false;
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Teacher disabled successfully!";
                return Json(new { success = true, message = "Teacher disabled successfully!" });
            }
            else
            {
                TempData["ErrorMessage"] = "Error disabling teacher. Teacher not found.";
            }

            return Json(new { success = false, message = "Teacher not found." });
        }

        [HttpPost]
        public JsonResult EnableTeacher(Guid id)
        {
            var reg = _context.Registrations.FirstOrDefault(r => r.RegdId == id);
            var acc = _context.Accounts.FirstOrDefault(a => a.RegdId == id);
            var roleId = _context.Roles.FirstOrDefault(r => r.RoleName == "Teacher")?.RoleId;
            var accRole = _context.AccountRoles.FirstOrDefault(ar => ar.AcId == acc.AcId && ar.RoleId == roleId);

            if (reg != null && acc != null && accRole != null)
            {
                reg.IsActive = true;
                acc.IsActive = true;
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Teacher enabled successfully!";
                return Json(new { success = true, message = "Teacher enabled successfully!" });
            }
            else
            {
                TempData["ErrorMessage"] = "Error enabling teacher. Teacher not found.";
            }

            return Json(new { success = false, message = "Teacher not found." });
        }
        #endregion

        #region Manage Student Region
        public IActionResult StudentIndex()
        {
            var teachers = _context.Registrations.Where(r => r.CreatedBy == "Student")
                           .Select(reg => new StudentViewModel
                           {
                               RegdId = reg.RegdId,
                               FirstName = reg.FirstName,
                               LastName = reg.LastName,
                               MobileNo = reg.MobileNo,
                               Email = reg.Email,
                               DateOfBirth = reg.DateOfBirth,
                               IsActive = reg.IsActive
                           }).ToList();

            return View(teachers);
        }

        public IActionResult UpdateStudent(Guid id)
        {
            var disableStudent = _context.Registrations.Where(r => r.RegdId == id && r.IsActive).FirstOrDefault();
            if (disableStudent == null)
            {
                TempData["ErrorMessage"] = "Please enable this student account first";
                return RedirectToAction("StudentIndex", TempData);
            }
            var student = (from reg in _context.Registrations
                           join acc in _context.Accounts on reg.RegdId equals acc.RegdId
                           where reg.RegdId == id
                           select new StudentViewModel
                           {
                               RegdId = reg.RegdId,
                               FirstName = reg.FirstName,
                               LastName = reg.LastName,
                               MobileNo = reg.MobileNo,
                               Email = reg.Email,
                               //DateOfBirth = DateTime.Parse(reg.DateOfBirth),
                               DateOfBirth = reg.DateOfBirth,
                           }).FirstOrDefault();

            if (student == null)
            {
                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction("StudentIndex");
            }

            return View(student);
        }

        [HttpPost]
        public IActionResult UpdateStudent(StudentViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DateOfBirth == null)
                {
                    TempData["ErrorMessage"] = "Please provide date of birth";
                    return View(model);
                }

                DateTime dob = DateTime.Parse(model.DateOfBirth);
                if (dob > DateTime.Now)
                {
                    TempData["ErrorMessage"] = "Date of birth can not be inserted after today";
                    return View(model);
                }

                var reg = _context.Registrations.FirstOrDefault(r => r.RegdId == model.RegdId && r.IsActive);
                var acc = _context.Accounts.FirstOrDefault(a => a.RegdId == model.RegdId && reg.IsActive);

                var checkDuplicateEmailEntry = _context.Registrations.Where(r => r.Email == model.Email && r.IsActive && (model.Email != reg.Email)).FirstOrDefault();
                if (checkDuplicateEmailEntry != null && (model.Email == checkDuplicateEmailEntry.Email))
                {
                    TempData["ErrorMessage"] = "Email address already exists";
                    return View(model);
                }

                if (reg != null && acc != null)
                {
                    reg.FirstName = model.FirstName;
                    reg.LastName = model.LastName;
                    reg.MobileNo = model.MobileNo;
                    reg.Email = model.Email;
                    reg.DateOfBirth = model.DateOfBirth;

                    acc.Email = model.Email;

                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Student updated successfully!";
                    return View();
                }

                TempData["ErrorMessage"] = "Student not found.";
                return View(model);
            }
            TempData["ErrorMessage"] = "Provide the valid student data";
            return View(model);
        }

        [HttpPost]
        public JsonResult DeleteStudent(Guid id)
        {
            var reg = _context.Registrations.FirstOrDefault(r => r.RegdId == id);
            var acc = _context.Accounts.FirstOrDefault(a => a.RegdId == id);
            var roleId = _context.Roles.FirstOrDefault(r => r.RoleName == "Student")?.RoleId;
            var accRole = _context.AccountRoles.FirstOrDefault(ar => ar.AcId == acc.AcId && ar.RoleId == roleId);

            if (reg != null && acc != null && accRole != null)
            {
                reg.IsActive = false;
                acc.IsActive = false;
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Student disabled successfully!";
                return Json(new { success = true, message = "Student disabled successfully!" });
            }
            else
            {
                TempData["ErrorMessage"] = "Error disabling student. Student not found.";
            }

            return Json(new { success = false, message = "Student not found." });
        }

        [HttpPost]
        public JsonResult EnableStudent(Guid id)
        {
            var reg = _context.Registrations.FirstOrDefault(r => r.RegdId == id);
            var acc = _context.Accounts.FirstOrDefault(a => a.RegdId == id);
            var roleId = _context.Roles.FirstOrDefault(r => r.RoleName == "Student")?.RoleId;
            var accRole = _context.AccountRoles.FirstOrDefault(ar => ar.AcId == acc.AcId && ar.RoleId == roleId);

            if (reg != null && acc != null && accRole != null)
            {
                reg.IsActive = true;
                acc.IsActive = true;
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Student enabled successfully!";
                return Json(new { success = true, message = "Student enabled successfully!" });
            }
            else
            {
                TempData["ErrorMessage"] = "Error enabling student. Student not found.";
            }

            return Json(new { success = false, message = "Student not found." });
        }
        #endregion
    }
}
