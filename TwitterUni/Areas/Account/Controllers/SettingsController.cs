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

        public SettingsController(IUserService userService, IImageService imageService)
        {
            _userService = userService;
            _imageService = imageService;
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
    }
}
