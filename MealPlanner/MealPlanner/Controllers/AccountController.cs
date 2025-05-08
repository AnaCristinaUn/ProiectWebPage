using Microsoft.AspNetCore.Mvc;

namespace MealPlanner.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
