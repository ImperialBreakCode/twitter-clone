using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TwitterUni.Areas.Account.Models.Settings;
using TwitterUni.Infrastructure.Constants;
using TwitterUni.Infrastructure.Filters;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Areas.Account.Controllers
{
    [Area("Account")]
    [Authorize]
    [SetupUserFilter]
    public class SettingsController : Controller
    {
        private readonly IImageService _imageService;
        private readonly IUserService _userService;
        private readonly Mapper _mapper;

        public SettingsController(IUserService userService, IImageService imageService, Mapper mapper)
        {
            _userService = userService;
            _imageService = imageService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            UserData user = _userService.GetUserByUserName(User.Identity.Name);

            SettingsViewModel settingsVM = new SettingsViewModel();
            settingsVM.User = user;

            return View(settingsVM);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SettingsViewModel settingsVM)
        {
            UserData? user = _userService.GetUserByUserName(User.Identity.Name);
            settingsVM.User = user;

            if (ModelState.IsValid)
            {
                if (User.Identity.Name != settingsVM.UserName)
                {
                    settingsVM.DeleteFormHasErrors = true;
                    ModelState.AddModelError(nameof(settingsVM.UserName), "Username is not correct");
                    return View(settingsVM);
                }

                bool validPassword = await _userService.CheckPassword(settingsVM.UserName, settingsVM.Password);
                if (validPassword)
                {
                    await _userService.SignOutUser();

                    if (Request.Cookies["SetUser"] != null)
                    {
                        Response.Cookies.Delete("SetUser");
                    }

                    IdentityResult result = await _userService.DeleteUser(user.Id);
                    if (result.Succeeded)
                    {
                        if (user.BackgroundPhoto != "default_background.jpg")
                        {
                            _imageService.DeleteUserImage(user.BackgroundPhoto);
                        }

                        if (user.ProfilePic != "default_prf_pic.png")
                        {
                            _imageService.DeleteUserImage(user.ProfilePic);
                        }

                        _imageService.DeleteAllUserTweetImages(user.UserName);

                        return RedirectToAction("Index", "Home", new { Area = "" });
                    }
                    else
                    {
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError(String.Empty, error.Description);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(nameof(settingsVM.Password), "Password is not valid");
                }
            }

            settingsVM.DeleteFormHasErrors = true;
            return View(settingsVM);
        }

        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changePassVM)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await _userService.ChangePassword(User.Identity.Name, changePassVM.CurrentPassword, changePassVM.NewPassword);

                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return View(changePassVM);
        }

        [HttpGet]
        public IActionResult ChangeUserInfo()
        {
            UserData? user = _userService.GetUserByUserName(User.Identity.Name);
            ChangeUserInfoViewModel userInfoVM = _mapper.Map<ChangeUserInfoViewModel>(user);
            return View(userInfoVM);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserInfo(ChangeUserInfoViewModel userInfoVM)
        {
            if (ModelState.IsValid)
            {
                UserData? user = _userService.GetUserByUserName(User.Identity.Name);

                string? doubleUserId = _userService.GetUserByUserName(userInfoVM.UserName)?.Id;
                bool newUserNameExists = User.Identity.Name != userInfoVM.UserName 
                    && doubleUserId is not null;

                if (newUserNameExists)
                {
                    ModelState.AddModelError(string.Empty, $"Username {userInfoVM.UserName} already exists");
                    return View(userInfoVM);
                }

                if (user is not null)
                {
                    if (User.Identity.Name != userInfoVM.UserName)
                    {
                        _imageService.UpdateTweetImagePath(User.Identity.Name, userInfoVM.UserName);
                    }

                    _mapper.Map(userInfoVM, user);
                    await _userService.UpdateUser(user, true);

                    return RedirectToAction(nameof(Index));
                }
            }

            return View(userInfoVM);
        }
    }
}
