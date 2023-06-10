using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterUni.Areas.Account.Models.Auth;
using TwitterUni.Infrastructure.Extensions;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Areas.Auth.Controllers
{
    [Area("Account")]
    public class AuthController : Controller
    {
        private IUserService _userService;
        private Mapper _mapper;
        private const string isSetUserCookieName = "SetUser";

        public AuthController(IUserService userService, Mapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                UserData userData = _mapper.Map<UserData>(registerViewModel);
                userData.IsSet = true;
                userData.ProfilePic = "default_prf_pic.png";
                userData.BackgroundPhoto = "default_background.jpg";

                var result = await _userService.CreateUser(userData, registerViewModel.Password);

                if (result.Succeeded)
                {
                    await _userService.SignInUser(registerViewModel.UserName, registerViewModel.Password);

                    Response.Cookies.AddAuthHelperCookie(isSetUserCookieName, true);

                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(registerViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.SignInUser(loginVM.UserName, loginVM.Password);

                if (result is not null && result.Succeeded)
                {
                    if (!_userService.GetUserByUserName(loginVM.UserName)?.IsSet ?? false)
                    {
                        Response.Cookies.AddAuthHelperCookie(isSetUserCookieName, false);
                        return RedirectToAction(nameof(Setup));
                    }

                    Response.Cookies.AddAuthHelperCookie(isSetUserCookieName, true);
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else if (result is not null && result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "User account locked out.");
                }
                else if(result is null)
                {
                    ModelState.AddModelError(string.Empty, $"User {loginVM.UserName} does not exist.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                }
            }

            return View(loginVM);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Setup()
        {
            var user = _userService.GetUserByUserName(User.Identity.Name);

            if (user is not null && !user.IsSet)
            {
                RegisterViewModel registerVM = _mapper.Map<RegisterViewModel>(user);
                return View(registerVM);
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Setup(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserByUserName(User.Identity.Name);

                string? doubleUser = _userService.GetUserByUserName(registerViewModel.UserName)?.Id;
                bool newUserNameExists = User.Identity.Name != registerViewModel.UserName 
                    && doubleUser is not null;

                if (user is not null && !newUserNameExists)
                {
                    _mapper.Map(registerViewModel, user);
                    await _userService.CompleteUserSetup(user, registerViewModel.Password);

                    if (Request.Cookies[isSetUserCookieName] != null)
                    {
                        Response.Cookies.Delete(isSetUserCookieName);
                    }

                    Response.Cookies.AddAuthHelperCookie(isSetUserCookieName, true);

                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Username {registerViewModel.UserName} is already taken.");
                }
            }

            return View(registerViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutUser();

            if (Request.Cookies[isSetUserCookieName] != null)
            {
                Response.Cookies.Delete(isSetUserCookieName);
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
