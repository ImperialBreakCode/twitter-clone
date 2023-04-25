using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public TweetController(ITagService tagService, ITweetService tweetService, IImageService imageService)
        {
            _tagService = tagService;
            _tweetService = tweetService;
            _imageService = imageService;
        }

        [HttpGet]
        public IActionResult One(string id)
        {
            TweetData? tweetData = _tweetService.GetTweet(id);

            if (tweetData is not null)
            {
                TweetViewModel tweetVM = new TweetViewModel();
                tweetVM.Tweet = tweetData;
                tweetVM.Tags = _tagService.GetTweetTags(id);

                return View();
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
                if (createVM.TextContent == null && createVM.Image is null)
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
                        _tagService.AddTagsToTweet(tweetId, tagNames.Select(x => x.Value).ToList());
                    }
                }
            }

            return RedirectToAction("Profile", "User", new { Id = User.Identity.Name });
        }
    }
}
