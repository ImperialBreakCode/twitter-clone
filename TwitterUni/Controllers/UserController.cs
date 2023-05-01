using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using TwitterUni.Infrastructure.Filters;
using TwitterUni.Infrastructure.Constants;
using TwitterUni.Models.User;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Controllers
{
    [Authorize(Roles = RoleNames.User)]
    [SetupUserFilter]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly Mapper _mapper;
        private readonly IImageService _imageService;
        private readonly ITagService _tagService;
        private readonly ITweetService _tweetService;

        public UserController(
            IUserService userService, 
            Mapper mapper, 
            IImageService imageService,
            ITagService tagService,
            ITweetService tweetService)
        {
            _userService = userService;
            _mapper = mapper;
            _imageService = imageService;
            _tagService = tagService;
            _tweetService = tweetService;
        }

        [HttpGet]
        public IActionResult All()
        {
            List<UserData> users = _userService.GetAllUsers().ToList();
            return View(users);
        }

        [HttpGet]
        public IActionResult Profile(string id)
        {
            var user = _userService.GetUserByUserName(id);

            if (user is not null) 
            {
                List<TweetData> tweets = _tweetService.GetTweetsByUser(id)
                    .Select(t =>
                    {
                        t.ProfileOrderingDate = t.CreatedAt;
                        return t;

                    }).ToList();

                List<TweetData> retweets = _tweetService.GetRetweetsByUser(id)
                    .Select(r =>
                    {
                        r.Tweet.ProfileOrderingDate = r.CreatedAt;
                        r.Tweet.IsRetweet = true;
                        return r.Tweet;

                    }).ToList();

                tweets.AddRange(retweets);

                UserProfileViewModel userVM = new UserProfileViewModel();
                userVM.User = user;
                userVM.OtherUsers = _userService.GetAllUsersWithFollows().Take(5).ToList();
                userVM.Tags = _tagService.GetAllTags().Take(5).ToList();
                userVM.UserTweets = tweets.OrderByDescending(t => t.ProfileOrderingDate).ToList();

                return View(userVM);
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Followers(string id)
        {
            UserData? userData = _userService.GetUserByUserName(id);

            if (userData is not null)
            {
                FollowsViewModel followsVM = new FollowsViewModel();
                followsVM.UserName = userData.UserName;
                followsVM.UserList = userData.FollowersCollection.Select(f => f.TheFollower).ToList();

                return View(followsVM);
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Followings(string id)
        {
            UserData? userData = _userService.GetUserByUserName(id);

            if (userData is not null)
            {
                FollowsViewModel followsVM = new FollowsViewModel();
                followsVM.UserName = userData.UserName;
                followsVM.UserList = userData.FollowingsCollection.Select(f => f.IsFollowing).ToList();

                return View(followsVM);
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var user = _userService.GetUserByUserName(User.Identity.Name);

            if (user is not null)
            {
                EditUserViewModel editVM = _mapper.Map<EditUserViewModel>(user);
                return View(editVM);
            }

            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel editVM)
        {
            UserData? userData = _userService.GetUserByUserName(User.Identity.Name);

            if (ModelState.IsValid && userData is not null)
            {
                Random r = new Random();
                string userImageName = $"{r.Next(0, 1000)}_{DateTime.UtcNow.ToFileTimeUtc()}.jpg";

                if (editVM.ProfilePicBase64 is not null)
                {
                    Match matches = Regex.Match(editVM.ProfilePicBase64, @"data:(?<type>.+?);base64,(?<data>.+)");
                    
                    if (matches.Groups.Count == 3)
                    {
                        byte[] imgBuffer = Convert.FromBase64String(matches.Groups["data"].Value);
                        using (Image image = Image.Load(new MemoryStream(imgBuffer)))
                        {
                            _imageService.SaveProfileImage(image, userImageName);
                        }

                        if (userData.ProfilePic != "default_prf_pic.png")
                        {
                            _imageService.DeleteUserImage(userData.ProfilePic);
                        }

                        userData.ProfilePic = $"{StaticFilePaths.ProfileImSubfolder}/{userImageName}";
                    }
                }

                if (editVM.BackgroundPhotoInput is not null)
                {
                    using (Image image = Image.Load(editVM.BackgroundPhotoInput.OpenReadStream()))
                    {
                        _imageService.SaveBackgroundImage(image, userImageName);
                    }

                    if (userData.BackgroundPhoto != "default_background.jpg")
                    {
                        _imageService.DeleteUserImage(userData.BackgroundPhoto);
                    }

                    userData.BackgroundPhoto = $"{StaticFilePaths.BgImSubfolder}/{userImageName}";
                }

                _mapper.Map(editVM, userData);
                await _userService.UpdateUser(userData);
                return RedirectToAction(nameof(Profile), new { Id = User.Identity.Name });
            }

            return View(editVM);
        }

        [HttpPost]
        public JsonResult FollowUser(string username)
        {
            var follower = _userService.GetUserByUserName(User.Identity.Name);
            var following = _userService.GetUserByUserName(username);

            if (follower is not null && following is not null)
            {
                _userService.FollowUser(follower.Id, following.Id);

                return new JsonResult(Ok());
            }

            Response.StatusCode = 404;
            return new JsonResult(NotFound("user not found"));
        }

        [HttpDelete]
        public JsonResult UnfollowUser(string username)
        {
            bool success = _userService.UnfollowUser(User.Identity.Name, username);

            if (success)
            {
                return new JsonResult(Ok());
            }

            Response.StatusCode = 404;
            return new JsonResult(NotFound("user not found"));
        }
    }
}
