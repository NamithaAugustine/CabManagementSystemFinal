using CabManagementSystems.Data;
using CabManagementSystems.Models;
using CabManagementSystems.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Data;

namespace CabManagementSystems.Areas.Account.Controllers
{
    [Area("Account")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public HomeController(ApplicationDbContext db,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Payment()
        {
            var book = _db.Bookings.Where(i => i.ApplicationUserId == _userManager.GetUserAsync(User).Result.Id).ToList();
            return View(book);
        }



        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> Wait(int id)
        //{
        //    Console.WriteLine("id is "+id);
        //    //var user = await _userManager.GetUserAsync(User);
        //    var booking = await _db.Bookings.FirstOrDefaultAsync(m => m.Id == id);
        //    //if(booking.DriverConfirmed==false)
        //    //{

        //    //}
        //    Console.WriteLine(booking.From);
        //    return View();
        //}



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid details");
                return View(model);
            }

            var res = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);
            if (res.Succeeded)
            {
                return RedirectToAction("Index", "Home", new { Area = "Account" });

            }
            ModelState.AddModelError(nameof(model.Password), "Invalid username/password");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber =model.PhoneNumber,
                Gender = model.Gender,
                UserName = Guid.NewGuid().ToString().Replace("-", ""),
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (var item in result.Errors)
            {
                ModelState.AddModelError("", item.Description);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { Area = "Account" });
        }

        public async Task<IActionResult> GenerateData()
        {
            await _roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            await _roleManager.CreateAsync(new IdentityRole() { Name = "User" });
            await _roleManager.CreateAsync(new IdentityRole() { Name = "Driver" });

            var users = await _userManager.GetUsersInRoleAsync("Admin");
            if (users.Count == 0)
            {
                var appUser = new ApplicationUser()
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@admin.com",
                    UserName = "admin",
                };
                var res = await _userManager.CreateAsync(appUser, "Pass@123");
                await _userManager.AddToRoleAsync(appUser, "Admin");
            }
            return Ok("Data generated");
        }
        //[Authorize(Roles = "User")]
        [HttpGet]
        public IActionResult Book()
        {

            return View();

        }
        //[Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> Book(BookingViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (model.From == model.To)
            {
                ModelState.AddModelError(nameof(model.To), "Invalid destination");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            IEnumerable<Place> locations =  _db.Places.ToList();
            int distance=0;
            foreach (var item in locations)
            {
                if (item.From == model.From && item.To == model.To)
                {
                    distance = item.Distance;
                }
            }
            _db.Bookings.Add(new Booking()
            {
                To = model.To,
                From = model.From,
                Date = model.Date,
                ApplicationUserId = user.Id,
                DriverId = "4c370f12-f455-4adc-9b51-9f4b1b0dce17",
                Distance = distance
            }) ;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { Area = "Account" });

        }




        [HttpGet]
        public IActionResult Type()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return NotFound();

            return View(new EditViewModel()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,

            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditViewModel model)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;

            await _userManager.UpdateAsync(user);
            //var result = await _userManager.CreateAsync(user, model.Password);
            await _db.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));,
            return RedirectToAction("Index", "Home", new { Area = "Account" });
        }

        public async Task<IActionResult> Delete()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            //var user = await _db.ApplicationUsers.FindAsync(id);
            if (user == null)
                return NotFound();

            _db.ApplicationUsers.Remove(user);
            await _db.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("Index", "Home", new { Area = "Account" });
        }

        public async Task<IActionResult> Pay(int id)
        {

            
            var book = await _db.Bookings.FindAsync(id);
            
            book.Payed= true;
            await _db.SaveChangesAsync();
            Console.WriteLine("Payment is"+book.Payed);
            return View(book);
        }

    }
}