using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterUni.Constants;
using TwitterUni.Filters;
using TwitterUni.Models.User;
using TwitterUni.Services.Interfaces;

namespace TwitterUni.Controllers
{
    [Authorize(Roles = RoleNames.User)]
    [SetupUserFilter()]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Profile(string id)
        {
            var user = _userService.GetUserByUserName(id);

            if (user is not null){

                UserProfileViewModel userVM = new UserProfileViewModel();
                userVM.User = user;
                userVM.OtherUsers = _userService.GetAllUsers().Take(5).ToList();

                return View(userVM);
            }

            return NotFound();
        }
    }
}
