using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using MonstaFinalProject.Models;
using MonstaFinalProject.ViewModels.RegisterViewModels;
using MonstaFinalProject.ViewModels.RegisterViewModels;

namespace MonstaFinalProject.Controllers
{
    public class RegisterController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _con;
        //private readonly SmtpSetting _smtpSetting;


        public RegisterController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, IConfiguration con)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _con = con;
        }

        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));

        //    return Ok("Rollls succesfully created");
        //}


        //public async Task<IActionResult> CreateSuperAdmin()
        //{
        //    AppUser appUser = new AppUser
        //    {
        //        Name = "Super",
        //        //Surname = "Admin",
        //        UserName = "Superadmin",
        //        Email = "superadmin@gmail.com"
        //    };
        //    await _userManager.CreateAsync(appUser,"SuperAdmin229");
        //    await _userManager.AddToRoleAsync(appUser, "SuperAdmin");

        //    return Ok("Super admin ugurla yaradildi");
        //}



        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser appUser = new AppUser
            {
                UserName = registerVM.UserName,
                Email = registerVM.Email,
                Name= registerVM.Name,
                
            };

            IdentityResult identityResult = await _userManager.CreateAsync(appUser, registerVM.Password);

            if (!identityResult.Succeeded)
            {
                foreach (IdentityError identityError in identityResult.Errors)
                {
                    ModelState.AddModelError("", identityError.Description);
                }
                return View(registerVM);
            }
            await _userManager.AddToRoleAsync(appUser, "Admin");

            MimeMessage mimeMessage = new MimeMessage();
            mimeMessage.From.Add(MailboxAddress.Parse(_con.GetSection("SmptSetting:Email").Value));
            mimeMessage.To.Add(MailboxAddress.Parse(appUser.Email));
            mimeMessage.Subject = "Email Confirmation";
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $"{appUser.Email} Confirm Email{appUser.Name}"
            };
            using (SmtpClient smtpClient = new SmtpClient())
            {
                await smtpClient.ConnectAsync(_con.GetSection("SmptSetting:Host").Value, int.Parse(_con.GetSection("SmptSetting:Port").Value), MailKit.Security.SecureSocketOptions.StartTls);
                await smtpClient.AuthenticateAsync(_con.GetSection("SmptSetting: Email").Value, _con.GetSection("SmptSetting: Password").Value);
                await smtpClient.SendAsync(mimeMessage);
                await smtpClient.DisconnectAsync(true);
                smtpClient.Dispose();
            }


            return View(new MonstaFinalProject.ViewModels.RegisterViewModels.ProfileVM());
            //return View(new MonstaFinalProject.ViewModels.LoginViewModels.LoginVM(nameof(login)));


            if (await _userManager.Users.AnyAsync(u => u.NormalizedUserName == registerVM.UserName.Trim().ToUpperInvariant()))
            {
                ModelState.AddModelError("UserName", $"{registerVM.UserName} Already Taken");
                return View(registerVM);
            }
            if (await _userManager.Users.AnyAsync(u => u.NormalizedEmail == registerVM.Email.Trim().ToUpperInvariant()))
            {
                ModelState.AddModelError("Email", $"{registerVM.Email} Already Taken");
                return View(registerVM);
            }
        }

       
    }
}
