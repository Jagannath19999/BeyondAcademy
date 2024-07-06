using Microsoft.AspNetCore.Mvc;

namespace BeyondAcademy.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult AdminDashboard()
        {
            return View();
        }
    }
}
