using CabManagementSystems.Data;
using CabManagementSystems.Models;
using CabManagementSystems.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CabManagementSystems.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class HomeController
        : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        

        public HomeController(ApplicationDbContext db, 
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
            //return View("Index");
        }

        [HttpGet]
        //[Route("[area]/login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[Route("[area]/login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid details.");
                return View(model);
            }

            var res = await _signInManager.PasswordSignInAsync(user, model.Password, true, true);

            if (res.Succeeded && model.Email == "admin@admin.com")
            {
                return RedirectToAction("Index", "Home", new {Area="Admin"});
                //return Redirect("/");
            }
            ModelState.AddModelError("", "Invalid email / password");
            return View(model);
        }

        public IActionResult Customer()
        {
            return View(_db.ApplicationUsers.ToList());
        }

        public IActionResult Driver()
        {
            return View(_db.Drivers.ToList());
        }

        public async Task<IActionResult> CustomerDelete(string id)
        {
            var user = await _db.ApplicationUsers.FindAsync(id);
            if (user == null)
                return NotFound();

            _db.ApplicationUsers.Remove(user);
            await _db.SaveChangesAsync();
            return RedirectToAction("Customer", "Home", new { Area = "Admin" });
        }
        public async Task<IActionResult> DriverDelete(string id)
        {
            var Cabdriver = await _db.Drivers.FindAsync(id);
            if (Cabdriver == null)
                return NotFound();

            _db.Drivers.Remove(Cabdriver);
            await _db.SaveChangesAsync();
            return RedirectToAction("Driver", "Home", new { Area = "Admin" });
        }



        [HttpGet]
        public async Task<IActionResult> DriverEdit(string id)
        {
            var movie = await _db.Drivers.FindAsync(id);
            if (movie == null)
                return NotFound();

            return View(new DriverEditViewModel()
            {
                Car = movie.Car,
                CarName= movie.CarName,
                CarNumber= movie.CarNumber,
                Dob= movie.Dob,
                Gender= movie.Gender,
            });
        }

        [HttpPost]
        public async Task<IActionResult> DriverEdit(string id, DriverEditViewModel model)
        {
            var movie = await _db.Drivers.FindAsync(id);
            if (movie == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            movie.Car = model.Car;
            movie.CarName = model.CarName;
            movie.CarNumber = model.CarNumber;
            movie.Dob = model.Dob;
            movie.Gender = model.Gender;
            await _db.SaveChangesAsync();
            return RedirectToAction("Driver", "Home", new { Area = "Admin" });
        }




        [HttpGet]
        public async Task<IActionResult> CustomerEdit(string id)
        {
            var movie = await _db.ApplicationUsers.FindAsync(id);
            if (movie == null)
                return NotFound();

            return View(new EditViewModel()
            {
                FirstName = movie.FirstName,
                LastName = movie.LastName,
                Email    = movie.Email,
                
            });
        }

        [HttpPost]
        public async Task<IActionResult> CustomerEdit(string id, EditViewModel model)
        {
            var user = await _db.ApplicationUsers.FindAsync(id);
            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            user.FirstName  = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            await _db.SaveChangesAsync();
            return RedirectToAction("Customer", "Home", new { Area = "Admin" });
        }





        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "Admin" });
        }

        public IActionResult Booking()
        {
            return View(_db.Bookings.ToList());
        }


        public IActionResult Location()
        {
            return View(_db.Places.ToList());
        }

        [HttpGet]
        public IActionResult CreateLocation()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateLocation(PlaceViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _db.Places.Add(new Place()
            {
                From = model.From,
                To = model.To,
                Distance = model.Distance,
    
            });
            await _db.SaveChangesAsync();
            return RedirectToAction("Location", "Home", new { Area = "Admin" });
        }

        public async Task<IActionResult> DeleteLocation(string id)
        {
            var place = await _db.Places.FindAsync(id);
            if (place == null)
                return NotFound();

            _db.Places.Remove(place);
            await _db.SaveChangesAsync();
            return RedirectToAction("Location", "Home", new { Area = "Admin" });
        }

    }
}
