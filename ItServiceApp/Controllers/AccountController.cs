using AutoMapper;
using ItServiceApp.Business.Services.Email;
using ItServiceApp.Core.ComplexTypes;
using ItServiceApp.Core.Identity;
using ItServiceApp.Core.ViewModels;
using ItServiceApp.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ItServiceApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IEmailSender emailSender,
            IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _mapper = mapper;
            CheckRoles();
        }

        private void CheckRoles()
        {
            foreach (var roleName in RoleNames.Roles)
            {
                if (!_roleManager.RoleExistsAsync(roleName).Result)
                {
                    var result = _roleManager.CreateAsync(new ApplicationRole()
                    {
                        Name = roleName
                    }).Result;
                }
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Password = string.Empty;
                model.ConfirmPassword = string.Empty;
                return View(model);
            }

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                ModelState.AddModelError(nameof(model.UserName), "Bu kullanıcı adı daha önce sisteme kayıt edilmiştir");
                return View(model);
            }
            user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                ModelState.AddModelError(nameof(model.Email), "Bu email daha önce sisteme kayıt edilmiştir");
                return View(model);
            }

            user = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                Name = model.Name,
                Surname = model.Surname
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //kullanıcıya rol atama
                var count = _userManager.Users.Count();

                result = await _userManager.AddToRoleAsync(user, count == 1 ? RoleNames.Admin : RoleNames.Passive);

                //if (count == 1) //admin
                //{
                //    result = await _userManager.AddToRoleAsync(user, RoleNames.Admin);
                //}
                //else //user
                //{
                //    result = await _userManager.AddToRoleAsync(user, RoleNames.User);
                //}

                //kullanıcıya email doğrulama gönderme
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                    protocol: Request.Scheme);

                var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Body =
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                    Subject = "Confirm your email"
                };

                await _emailSender.SendAsync(emailMessage);


                //TODO:giriş sayfasına yönlendirme
            }
            else
            {
                ModelState.AddModelError(string.Empty, ModelState.ToFullErrorString());
                return View(model);
            }
            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            ViewBag.StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";

            if (result.Succeeded && _userManager.IsInRoleAsync(user, RoleNames.Passive).Result)
            {
                await _userManager.RemoveFromRoleAsync(user, RoleNames.Passive);
                await _userManager.AddToRoleAsync(user, RoleNames.User);
            }

            return View();
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, true);

            if (result.Succeeded)
            {
                //await _emailSender.SendAsync(new EmailMessage()
                //{
                //    Contacts = new string[] { "abc@ww.com" },
                //    Body = $"{HttpContext.User.Identity.Name} Sisteme giriş yaptı!",
                //    Subject = $"Merhaba {HttpContext.User.Identity.Name}"
                //});

                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı");
                return View(model);
            }
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            //var model = new UserProfileViewModel()
            //{
            //    Email = user.Email,
            //    Name = user.Name,
            //    Surname = user.Surname
            //};

            var model = _mapper.Map<UserProfileViewModel>(user);


            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            //var userModel = _mapper.Map<ApplicationUser>(model);

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            user.Name = model.Name;
            user.Surname = model.Surname;

            if (user.Email != model.Email)//mail adresini değiştirmiş!!!
            {
                await _userManager.RemoveFromRoleAsync(user, RoleNames.User);
                await _userManager.AddToRoleAsync(user, RoleNames.Passive);

                user.Email = model.Email;
                user.EmailConfirmed = false;

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                    protocol: Request.Scheme);

                var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Body =
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                    Subject = "Confirm your email"
                };

                await _emailSender.SendAsync(emailMessage);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, ModelState.ToFullErrorString());
            }

            return View(model);
        }

        public IActionResult PasswordUpdate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PasswordUpdate(PasswordUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId());

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                //email gönder

                TempData["Message"] = "Şifre değiştirme işleminiz başarılı";
                return View();
            }
            else
            {
                var message = string.Join("<br>", result.Errors.Select(x => x.Description));
                TempData["Message"] = message;
                return View();
            }
        }
        [AllowAnonymous]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                ViewBag.Message = "Girdiğiniz email sistemimizde bulunamadı";
            }
            else
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action("ConfirmResetPassword", "Account", new { userId = user.Id, code = code },
                    protocol: Request.Scheme);

                var emailMessage = new EmailMessage()
                {
                    Contacts = new string[] { user.Email },
                    Body =
                        $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.",
                    Subject = "Reset Password"
                };
                await _emailSender.SendAsync(emailMessage);
                ViewBag.Message = "Mailinize Şifre güncelleme yönergemiz gönderilmiştir";
            }

            return View();
        }
        [AllowAnonymous]
        public IActionResult ConfirmResetPassword(string userId, string code)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
            {
                return BadRequest("Hatalı istek");
            }

            ViewBag.Code = code;
            ViewBag.UserId = userId;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı");
                return View();
            }
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(model.Code));
            var result = await _userManager.ResetPasswordAsync(user, code, model.NewPassword);

            if (result.Succeeded)
            {
                //email gönder
                TempData["Message"] = "Şifre değişikliğiniz gerçekleştirilmiştir";
                return View();
            }
            else
            {
                var message = string.Join("<br>", result.Errors.Select(x => x.Description));
                TempData["Message"] = message;
                return View();
            }
        }

    }
}