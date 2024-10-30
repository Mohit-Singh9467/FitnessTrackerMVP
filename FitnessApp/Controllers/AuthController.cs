using FitnessApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Controllers
{


    public class AuthController : Controller
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Auth/SignUp
        public IActionResult SignUp()
        {
            return View();
        }

        // POST: Auth/SignUp
        [HttpPost]
        public async Task<IActionResult> SignUp(User user)
        {
            if (ModelState.IsValid)
            {
                // Check if the user already exists
                if (_context.Users.Any(u => u.Name == user.Name))
                {
                    ModelState.AddModelError("Username", "Username is already taken.");
                    return View(user);
                }
                user.EnrollmentDate = DateTime.Now;
                // For simplicity, we are not hashing the password here. You should hash the password in production.
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }

            return View(user);
        }

        // GET: Auth/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Auth/Login
        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.FirstOrDefault(u => u.Name == username && u.Password == password);

                if (user != null)
                {
                    // Store user information in session or implement authentication
                    HttpContext.Session.SetString("Username", username);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }


            return View();
        }


        // Logout Action
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

