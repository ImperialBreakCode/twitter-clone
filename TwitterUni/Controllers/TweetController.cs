using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.RegularExpressions;
using TwitterUni.Infrastructure.Constants;
using TwitterUni.Infrastructure.Filters;
using TwitterUni.Models.Tweet;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Controllers
{
    [Authorize(Roles = RoleNames.User)]
    [SetupUserFilter]
    public class TweetController : Controller
    {
        private readonly ITweetService _tweetService;
        private readonly ITagService _tagService;
        private readonly IImageService _imageService;
        private readonly ICommentService _commentService;
        private readonly IUserService _userService;
        public TweetController(
            ITagService tagService, 
            ITweetService tweetService, 
            IImageService imageService, 
            ICommentService commentService, 
            IUserService userService)
        {
            _tagService = tagService;
            _tweetService = tweetService;
            _imageService = imageService;
            _commentService = commentService;
            _userService = userService;
        }

        [HttpGet]
        [Route("[controller]/[action]/{id}/{fromPage}")]
        public IActionResult One(string id, string fromPage)
        {
            TweetData? tweetData = _tweetService.GetTweet(id);

            if (tweetData is not null)
            {
                TweetViewModel tweetVM = new TweetViewModel();
                tweetVM.Tweet = tweetData;
                tweetVM.Tags = _tagService.GetTweetTags(id);
                tweetVM.Comments = _commentService.GetTweetComments(id);
                tweetVM.CurrentUser = _userService.GetUserByUserName(User.Identity.Name);

                return View(tweetVM);
            }

            return NotFound();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateTweetViewModel createVM)
        {
            string username = User.Identity.Name;

            if (ModelState.IsValid)
            {
                if (createVM.TextContent == null && createVM.Image == null)
                {
                    ModelState.AddModelError(String.Empty, "Both inputs cannot be empty");
                    return View(createVM);
                }
                else
                {
                    TweetData tweetData = new TweetData();
                    MatchCollection? tagNames = null;

                    if (createVM.TextContent is not null)
                    {
                        tweetData.TextContent = createVM.TextContent;
                        tagNames = Regex.Matches(createVM.TextContent, @"#\w+");
                    }

                    if (createVM.Image is not null)
                    {
                        Random random = new Random();
                        string imageName = $"{random.Next(0, 1000)}_{DateTime.Now.ToFileTimeUtc()}.jpg";

                        using (Image image = Image.Load(createVM.Image.OpenReadStream()))
                        {
                            _imageService.SaveTweetImage(image, imageName, username);
                            tweetData.Image = $"{username}/{imageName}";
                        }
                    }

                    string tweetId = _tweetService.CreateTweet(tweetData, username).Id;

                    if (tagNames is not null)
                    {
                        _tagService.AddTagsToTweet(tweetId, tagNames.Select(x => x.Value).Distinct().ToList());
                    }

                    return RedirectToAction("Profile", "User", new { Id = User.Identity.Name });
                }
            }

            return View(createVM);
        }

        [HttpPost]
        public JsonResult LikeTweet(string tweetId)
        {
            bool isSuccess = _tweetService.LikeTweet(User.Identity.Name, tweetId);

            if (isSuccess)
            {
                return new JsonResult(Ok());
            }

            return new JsonResult(NotFound());
        }

        [HttpDelete]
        public JsonResult UnlikeTweet(string tweetId)
        {
            bool isSuccess = _tweetService.UnlikeTweet(User.Identity.Name, tweetId);

            if (isSuccess)
            {
                return new JsonResult(Ok());
            }

            return new JsonResult(NotFound());
        }

        [HttpPost]
        public JsonResult CreateRetweet(string tweetId)
        {
            bool isSuccess = _tweetService.CreateRetweet(User.Identity.Name, tweetId);

            if (isSuccess)
            {
                return new JsonResult(Ok());
            }

            return new JsonResult(NotFound());
        }

        [HttpDelete]
        public JsonResult DeleteRetweet(string tweetId)
        {
            bool isSuccess = _tweetService.DeleteRetweet(User.Identity.Name, tweetId);

            if (isSuccess)
            {
                return new JsonResult(Ok());
            }

            return new JsonResult(NotFound());
        }

        [HttpDelete]
        public JsonResult DeleteTweet(string tweetId)
        {
            var tweet = _tweetService.GetTweet(tweetId);

            if (tweet == null)
            {
                Response.StatusCode = 404;
                return new JsonResult(NotFound());
            }

            if (tweet.Author.UserName != User.Identity.Name)
            {
                Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return new JsonResult("Cannot delete tweet.");
            }
            
            _tweetService.DeleteTweet(tweetId);
            _tagService.DeleteEmptyTags();

            if (tweet.Image is not null)
            {
                _imageService.DeleteTweetImage(tweet.Image);
            }

            return new JsonResult(Ok());
        }
    }
}
