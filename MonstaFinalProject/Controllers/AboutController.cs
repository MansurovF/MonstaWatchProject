using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.Models;
//using MonstaFinalProject.ViewModels.ProfileViewModels;
using System.Net;

namespace MonstaFinalProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _con;
        //private readonly SmtpSetting _smtpSetting;


        public AccountController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IConfiguration con)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _con = con;
        }
        public async Task<IActionResult> Index()
        {
            
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return Ok();
            //return RedirectToAction(nameof(Login));
            //return View(new MonstaFinalProject.Controllers.LoginController());
        }

        //[HttpGet]
        //public async Task<IActionResult> Profile()
        //{
        //    AppUser appUser = await _userManager.FindByNameAsync(User.Identity.Name);

        //    ProfileVM profileVM = new ProfileVM
        //    {

        //        Name = appUser.Name,
        //        Email = appUser.Email,
        //        UserName = appUser.UserName,

        //    };

        //    return View(profileVM);
        //}


    }
}