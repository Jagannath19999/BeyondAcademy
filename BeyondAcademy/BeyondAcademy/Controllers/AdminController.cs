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
                return RedirectToAction("Login","Home");
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

        [HttpPost]
        public IActionResult UpdateProfile(AdminDetailsViewModel model)
        {
            bool isValid = true;

            if (model.FirstName.Any(char.IsDigit) || model.LastName.Any(char.IsDigit))
            {
                ModelState.AddModelError("", "First Name and Last Name cannot contain numbers.");
                isValid = false;
            }

            if (!Regex.IsMatch(model.MobileNo, @"^\d{10}$"))
            {
                ModelState.AddModelError("MobileNo", "Mobile Number must be exactly 10 digits.");
                isValid = false;
            }

            try
            {
                var mailAddress = new MailAddress(model.Email);
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Email", "Invalid Email Address.");
                isValid = false;
            }

            DateTime dateOfBirth;
            if (!DateTime.TryParse(model.DateOfBirth.ToString(), out dateOfBirth))
            {
                ModelState.AddModelError("DateOfBirth", "Invalid Date of Birth format.");
                isValid = false;
            }
            else if (dateOfBirth > DateTime.Today)
            {
                ModelState.AddModelError("DateOfBirth", "Date of Birth cannot be in the future.");
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

            TempData["ErrorMessage"] = "Registration or account not found.";
            return View(model);
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
        public IActionResult UpdateDetails(AdminDetailsViewModel model)
        {
            // Manual validation
            bool isValid = true;

            // Check if FirstName and LastName are valid (no numbers allowed)
            if (model.FirstName.Any(char.IsDigit) || model.LastName.Any(char.IsDigit))
            {
                ModelState.AddModelError("", "First Name and Last Name cannot contain numbers.");
                isValid = false;
            }

            // Check if MobileNo is a valid 10-digit number
            if (!Regex.IsMatch(model.MobileNo, @"^\d{10}$"))
            {
                ModelState.AddModelError("MobileNo", "Mobile Number must be exactly 10 digits.");
                isValid = false;
            }

            // Check if Email is a valid email address
            try
            {
                var mailAddress = new MailAddress(model.Email);
            }
            catch (FormatException)
            {
                ModelState.AddModelError("Email", "Invalid Email Address.");
                isValid = false;
            }

            DateTime dateOfBirth;
            if (!DateTime.TryParse(model.DateOfBirth.ToString(), out dateOfBirth))
            {
                ModelState.AddModelError("DateOfBirth", "Invalid Date of Birth format.");
                isValid = false;
            }
            else if (dateOfBirth > DateTime.Today)
            {
                ModelState.AddModelError("DateOfBirth", "Date of Birth cannot be in the future.");
                isValid = false;
            }

            if (!isValid || !ModelState.IsValid)
            {
                // Collect error messages from ModelState
                var errorMessages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                TempData["ErrorMessage"] = string.Join(" | ", errorMessages);

                // Return to the same view with the validation messages
                return View(model);
            }

            var registration = _context.Registrations.FirstOrDefault(r => r.RegdId == model.RegdId);
            var account = _context.Accounts.FirstOrDefault(a => a.AcId == model.AcId);

            if (registration != null && account != null)
            {
                // Update the registration and account details
                registration.FirstName = model.FirstName;
                registration.LastName = model.LastName;
                registration.MobileNo = model.MobileNo;
                registration.Email = model.Email;
                registration.DateOfBirth = model.DateOfBirth;

                account.UserId = model.UserId;

                // Save changes to the database
                _context.SaveChanges();

                // Set a success message and redirect to the UpdateProfile action
                TempData["SuccessMessage"] = "Profile updated successfully!";
                return RedirectToAction("UpdateProfile", new { AcId = model.AcId, RegdId = model.RegdId });
            }

            // If registration or account is not found, set an error message and return to the view
            TempData["ErrorMessage"] = "Registration or account not found.";
            return View(model);
        }


    }
}
