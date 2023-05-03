using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TwitterUni.Areas.Admin.Models.Home;
using TwitterUni.Infrastructure.Constants;
using TwitterUni.Infrastructure.Filters;
using TwitterUni.Services.Interfaces;

namespace TwitterUni.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = RoleNames.Admin)]
    [SetupUserFilter]
    public class HomeController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IUserService _userService;
        private readonly ITweetService _tweetService;
        private readonly ICommentService _commentService;
        private readonly IAppSettingsService _appSettingsService;

        public HomeController(
            ITagService tagService, 
            IUserService userService, 
            ITweetService tweetService, 
            ICommentService commentService, 
            IAppSettingsService appSettingsService)
        {
            _tagService = tagService;
            _userService = userService;
            _tweetService = tweetService;
            _commentService = commentService;
            _appSettingsService = appSettingsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HomeViewModel homeVM = new HomeViewModel();
            homeVM.UserCount = _userService.GetAllUsers().Count();
            homeVM.TweetCount = _tweetService.GetAllTweets().Count;
            homeVM.TagCount = _tagService.GetAllTags().Count;
            homeVM.CommentCount = _commentService.GetAllComments().Count;
            homeVM.Users = _userService.GetAllUsers().ToList();

            return View(homeVM);
        }

        [HttpGet]
        public IActionResult DataLoading()
        {
            DataLoadingViewModel dataLoadingVM = new DataLoadingViewModel();
            dataLoadingVM.DataIsLoaded = _appSettingsService.IsDataLoaded();

            return View(dataLoadingVM);
        }

        [HttpPost]
        public IActionResult FetchApiData()
        {
            return RedirectToAction(nameof(DataLoading));
        }
    }
}
