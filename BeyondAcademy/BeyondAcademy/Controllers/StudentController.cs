using Microsoft.AspNetCore.Mvc;

namespace BeyondAcademy.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult StudentDashboard()
        {
            return View();
        }
    }
}
