﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
                userData.IsSet = true;

                var result = await _userService.CreateUser(userData, registerViewModel.Password);

                if (result.Succeeded)
                {
                    await _userService.SignInUser(registerViewModel.UserName, registerViewModel.Password);
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
                        return RedirectToAction(nameof(Setup), new {id = loginVM.UserName});
                    }

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
        [Authorize]
        public IActionResult Setup(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid && registerViewModel.Id is not null)
            {
                var user = _userService.GetUserById(registerViewModel.Id);
                var doubleUserId = _userService.GetUserByUserName(registerViewModel.UserName)?.Id;

                if (user is not null && doubleUserId is not null)
                {
                    if (user.Id == doubleUserId)
                    {
                        _mapper.Map(registerViewModel, user);
                        _userService.CompleteUserSetup(user, registerViewModel.Password);

                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Username {registerViewModel.UserName} is already taken.");
                    }
                }
            }

            return View(registerViewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _userService.SignOutUser();

            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
