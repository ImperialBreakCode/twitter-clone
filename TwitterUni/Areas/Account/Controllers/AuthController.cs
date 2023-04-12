using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TwitterUni.Areas.Account.Models.Auth;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services.ModelData;

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
                bool isCreated = await _userService.CreateUser(userData, registerViewModel.Password);

                if (isCreated)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }

            foreach (var item in ModelState)
            {
                foreach (var error in item.Value.Errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }
            }

            registerViewModel.Password = String.Empty;
            registerViewModel.ConfirmPassword = String.Empty;
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
                bool loggedIn = await _userService.SignInUser(loginVM.UserName, loginVM.Password);

                if (loggedIn)
                {
                    var user = _userService.GetUserByUserName(loginVM.UserName);
                    if (user is not null && !user.IsSet)
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
            if (ModelState.IsValid && registerViewModel.Id is not null)
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
