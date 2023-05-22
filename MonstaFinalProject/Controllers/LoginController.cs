using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MonstaFinalProject.Models;
using MonstaFinalProject.ViewModels.LoginViewModels;

namespace MonstaFinalProject.Controllers
{
    public class LoginController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _con;

        public LoginController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IConfiguration con)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _con = con;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);
            AppUser appUser = await _userManager.FindByEmailAsync(loginVM.Email);

            if (appUser == null)
            {
                ModelState.AddModelError("", "Email ve ya Sifre Yalnisdir");
                return View(loginVM);
            }
            if (!await _userManager.CheckPasswordAsync(appUser,loginVM.Password))
            {
                ModelState.AddModelError("", "Email ve ya Sifre Yalnisdir");
                return View(loginVM);
            }

            Microsoft.AspNetCore.Identity.SignInResult signInResult = await _signInManager
                .PasswordSignInAsync(appUser,loginVM.Password,false,true);

            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Hesabiniz bloklanib");
                return View(loginVM);
            }

            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Email ve ya Sifre Yalnisdir");
                return View(loginVM);
            }

            return RedirectToAction("index", "home");

        }
    }
}
