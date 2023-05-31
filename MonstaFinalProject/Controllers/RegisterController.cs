using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using MonstaFinalProject.Models;
using MonstaFinalProject.ViewModels.RegisterViewModels;
using MonstaFinalProject.ViewModels.LoginViewModels;
using MonstaFinalProject.DataAccessLayer;
using MonstaFinalProject.ViewModels;
using System.Security.Policy;
using NuGet.Protocol.Plugins;

namespace MonstaFinalProject.Controllers
{
    public class RegisterController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IConfiguration _con;
        private readonly SmtpSetting _smtpSetting;


        public RegisterController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, AppDbContext context, IConfiguration con, IOptions<SmtpSetting> smtpSetting)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _signInManager = signInManager;
            _con = con;
            _context = context;
            _smtpSetting = smtpSetting.Value;
        }

        



        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(RegisterVM registerVM)
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
            await _userManager.AddToRoleAsync(appUser, "Member");
            //string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            //string url = Url.Action("EmailConfirm", "Account", new { id = appUser.Id, token = token }, HttpContext.Request.Scheme, HttpContext.Request.Host.ToString());

            //string templateFullPath = Path.Combine(Directory.GetCurrentDirectory(), "Views", "Shared", "_EmailConfirm.cshtml");
            //string templateContent = await System.IO.File.ReadAllTextAsync(templateFullPath);
            ////templateContent = templateContent.Replace("{{email}}", appUser.Email);
            //templateContent = templateContent.Replace("{{username}}", appUser.UserName);
            //templateContent = templateContent.Replace("{{url}}", url);

            //MimeMessage mimeMessage = new MimeMessage();
            //mimeMessage.From.Add(MailboxAddress.Parse(_smtpSetting.Email));
            //mimeMessage.To.Add(MailboxAddress.Parse(appUser.Email));
            //mimeMessage.Subject = "Email Confirmation";
            //mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            //{
            //    Text = templateContent
            //};
            //using (SmtpClient smtpClient = new SmtpClient())
            //{
            //    await smtpClient.ConnectAsync(_smtpSetting.Host, _smtpSetting.Port, MailKit.Security.SecureSocketOptions.StartTls);
            //    await smtpClient.AuthenticateAsync(_smtpSetting.Email, _smtpSetting.Password);
            //    await smtpClient.SendAsync(mimeMessage);
            //    await smtpClient.DisconnectAsync(true);
            //    smtpClient.Dispose();
            //}


            return View(new MonstaFinalProject.ViewModels.RegisterViewModels.RegisterVM());
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
        [HttpGet]
        public async Task<IActionResult> EmailConfirm(string? id, string? token)
        {
            if (string.IsNullOrWhiteSpace(id)) return BadRequest();
            if (string.IsNullOrWhiteSpace(token)) return BadRequest();
            AppUser appUser = await _userManager.FindByIdAsync(id);
            if (appUser == null) return NotFound();
            IdentityResult identityResult = await _userManager.ConfirmEmailAsync(appUser, token);
            if (!identityResult.Succeeded) return BadRequest();
            TempData["Success"] = "Your Email Is Confirmed Successfully!";
            return RedirectToAction(nameof(Index));
        }


    }
}
