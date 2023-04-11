using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TwitterUni.Areas.Account.Models.Auth;
using TwitterUni.Services.Interfaces;

namespace TwitterUni.Areas.Auth.Controllers
{
    [Area("Account")]
    public class AuthController : Controller
    {
        private IUserService _userService;
        private Mapper _mapper;

        public AuthController(IUserService userService, Mapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
                bool loggedIn = await _userService.SignInUser(loginVM.UserName, loginVM.Password);

                if (loggedIn)
                {
                    if (!_userService.GetUserByUserName(loginVM.UserName).IsSet)
                    {
                        return RedirectToAction(nameof(Setup), new {id = loginVM.UserName});
                    }

                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Setup(string id)
        {
            var user = _userService.GetUserByUserName(id);

            if (user is not null && !user.IsSet)
            {
                RegisterViewModel registerVM = _mapper.Map<RegisterViewModel>(user);
                return View(registerVM);
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Setup(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUserById(registerViewModel.Id);

                if (user is not null)
                {
                    _mapper.Map(registerViewModel, user);
                    _userService.CompleteUserSetup(user, registerViewModel.Password);

                    return RedirectToAction("Index", "Home", new {area = ""});
                }
            }

            registerViewModel.Password = String.Empty;
            registerViewModel.ConfirmPassword = String.Empty;
            return View(registerViewModel);
        }
    }
}
