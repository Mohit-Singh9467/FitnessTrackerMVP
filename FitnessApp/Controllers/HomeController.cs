using FitnessApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FitnessApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;
        private User user;
        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context= context;
        }

        public IActionResult Index()
        {
           if(HttpContext.Session.GetString("Username")!=null)
            {
                user = _context.Users.Include(x => x.Goals).Include(x=>x.Workout).FirstOrDefault(x => x.Name == (HttpContext.Session.GetString("Username"))); ;
 
               
                    return View("Index",user);
                
            }
          
           else
                return View("../Auth/Login");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult AddGoal()
        {
          
            return View();  // Pass userId to the view to associate with the goal
           
        }
        [HttpPost]
        public IActionResult AddGoal(Goal goal)
        {
            if (ModelState.IsValid)
            {

               user= _context.Users.FirstOrDefault(x => x.Name == (HttpContext.Session.GetString("Username")));
                if(user!=null)
                {
                    if(user.Goals==null)
                    {
                        user.Goals = new List<Goal>();

                    }
                    user.Goals.Add(goal);
                }
              
                _context.SaveChanges();  // Save to the database
                return RedirectToAction("Index");
            }
            return View(goal);
        }
        [HttpPost]
        public IActionResult EditWorkout(Workout workoutPlan)
        {
            if (ModelState.IsValid)
            {
                user = _context.Users.FirstOrDefault(x => x.Name == (HttpContext.Session.GetString("Username")));
                if (user != null)
                {

                    user.Workout = workoutPlan;
                }
               
                _context.SaveChanges();  // Update the workout plan in the database
                return RedirectToAction("Index");
            }
            return View(workoutPlan);
        }
        public IActionResult EditWorkout()
        {
            // Logic to edit workout
            return View();
        }
    }
}