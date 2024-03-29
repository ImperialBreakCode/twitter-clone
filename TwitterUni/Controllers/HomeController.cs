﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TwitterUni.Infrastructure.Constants;
using TwitterUni.Infrastructure.Filters;
using TwitterUni.Models;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly ITagService _tagService;
        private readonly ITweetService _tweetService;

        public HomeController(
            ILogger<HomeController> logger, 
            IUserService userService, 
            ITagService tagService,
            ITweetService tweetService)
        {
            _logger = logger;
            _userService = userService;
            _tagService = tagService;
            _tweetService = tweetService;
        }

        public IActionResult Index()
        {
            List<UserData> users = _userService.GetAllUsersWithFollows().Take(5).ToList();
            List<TagData> tags = _tagService.GetAllTags().Take(5).ToList();
            List<TweetData> tweets = _tweetService.GetAllTweets().OrderByDescending(t => t.CreatedAt).ToList();
            
            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Users = users;
            homeViewModel.Tags = tags;
            homeViewModel.Tweets = tweets;

            return View(homeViewModel);
        }

        [Authorize(Roles = RoleNames.User)]
        [SetupUserFilter]
        public IActionResult Following()
        {
            List<UserData> users = _userService.GetAllUsersWithFollows().Take(5).ToList();
            List<TagData> tags = _tagService.GetAllTags().Take(5).ToList();

            List<TweetData> tweets = new List<TweetData>();
            List<string> userFollowings = _userService.GetUserByUserName(User.Identity.Name)
                .FollowingsCollection
                .Select(f => f.IsFollowing.UserName)
                .ToList();

            foreach (string username in userFollowings)
            {
                tweets.AddRange(_tweetService.GetTweetsByUser(username));
            }

            tweets.OrderByDescending(t => t.CreatedAt);

            HomeViewModel homeViewModel = new HomeViewModel();
            homeViewModel.Users = users;
            homeViewModel.Tags = tags;
            homeViewModel.Tweets = tweets;

            return View("Index", homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}