using Microsoft.AspNetCore.Mvc;

namespace BeyondAcademy.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult TeacherDashboard()
        {
            return View();
        }

    }
}
