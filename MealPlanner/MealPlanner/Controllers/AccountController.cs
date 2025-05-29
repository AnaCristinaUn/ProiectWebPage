using Microsoft.AspNetCore.Mvc;
using MealPlanner.Data;
using MealPlanner.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;

public class AccountController : Controller
{
    private readonly MealPlannerContext _context;

    public AccountController(MealPlannerContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult Login()
    {
        ViewBag.Error = null;
        return View();
    }

    [HttpPost]
    public ActionResult Login(string Email, string Password)
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ViewBag.Error = "Please fill in all fields.";
            return View();
        }

        var user = _context.User.FirstOrDefault(u => u.Email == Email);
        if (user != null)
        {
            if (user.Password == Password)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                int? userId = HttpContext.Session.GetInt32("UserId");
                return RedirectToAction("Dashboard", "Home");
            }
        }

        ViewBag.Error = "Invalid credentials.";
        return View();
    }

    [HttpGet]
    public IActionResult CreateAccount()
    {
        ViewBag.Error = null;
        return View();
    }

    [HttpPost]
    public IActionResult CreateAccount(string FirstName, string LastName, string Email, string Password, int? Age)
    {
        if (string.IsNullOrWhiteSpace(FirstName) ||
            string.IsNullOrWhiteSpace(LastName) ||
            string.IsNullOrWhiteSpace(Email) ||
            string.IsNullOrWhiteSpace(Password))
        {
            ViewBag.Error = "Please fill in all required fields.";
            return View();
        }

        if (_context.User.Any(u => u.Email == Email))
        {
            ViewBag.Error = "Email already in use.";
            return View();
        }

        var user = new User
        {
            FirstName = FirstName,
            LastName = LastName,
            Email = Email,
            Password = Password,
            Age = Age
        };

        _context.User.Add(user);
        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Details()
    {
        int? userId = HttpContext.Session.GetInt32("UserId");

        if (!userId.HasValue)
        {
            return RedirectToAction("Login");
        }

        var user = _context.User.FirstOrDefault(u => u.Id == userId.Value);
        if (user == null)
        {
            return RedirectToAction("Login");
        }

        var model = new
        {
            
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            Age = user.Age
        };

        return View(model);
    }

}
