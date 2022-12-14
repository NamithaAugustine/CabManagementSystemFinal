using CabManagementSystems.Data;
using CabManagementSystems.Models;
using CabManagementSystems.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CabManagementSystems.Areas.Driver.Controllers
{
    [Area("Driver")]
    public class HomeController : Controller
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

        //public Task<IActionResult> Confirm(Booking model)
        //{
        //    var user = await _userManager.FindByNameAsync(User.Identity.Name);

        //   var d = _db.Drivers.ToList();

        //    foreach (var item in d)
        //    {
        //        if(user.Id== item.ApplicationUserId)
        //        {
        //            model.DriverId = item.Id;
        //        }
        //    }

        //    return View(user);
        //}

        public async Task<IActionResult> ShowRide()
        {
            var book = _db.Bookings.Where(i => i.ApplicationUserId == _userManager.GetUserAsync(User).Result.Id).ToList();
            return View(book);
        }

        public IActionResult DriverIndex()
        {
            return View(_db.Bookings.ToList());
            //return View("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Confirm(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            var cabDriver = await _db.Drivers.FirstOrDefaultAsync(m => m.ApplicationUserId == user.Id);
            var booking = await _db.Bookings.FirstOrDefaultAsync(m=>m.Id == id);
            
            booking.DriverId = cabDriver.Id;
            booking.DriverConfirmed = true;
            
            await _db.SaveChangesAsync();
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            return View(user);
        }


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
                return RedirectToAction("DriverIndex", "Home", new { Area = "Driver" });

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
        public async Task<IActionResult> Register(DriverRegisterViewModel model)
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
                Gender= model.Gender,
                PhoneNumber = model.PhoneNumber,
                UserName = Guid.NewGuid().ToString().Replace("-", ""),
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            var user1 = new DriverDetails
            {
                Dob = model.Dob,
                Car = model.Car,
                Gender = model.Gender,
                CarName = model.CarName,
                CarNumber = model.CarNumber,
                ApplicationUserId = user.Id,
            };
            if (result.Succeeded)
            {
                _db.Drivers.Add(user1);
                await _db.SaveChangesAsync();
                return RedirectToAction("DriverIndex", "Home", new { Area = "Driver" });
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
            return RedirectToAction("DriverIndex", "Home", new { Area = "Driver" });
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
            return RedirectToAction("DriverIndex", "Home", new { Area = "Driver" });
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
            return RedirectToAction("DriverIndex", "Home", new { Area = "Driver" });
        }

    }
}

