using Microsoft.AspNetCore.Mvc;
using TwitterUni.Models.Tag;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Controllers
{
    public class TagController : Controller
    {
        private readonly ITagService _tagService;
        private readonly IUserService _userService;

        public TagController(ITagService tagService, IUserService userService)
        {
            _tagService = tagService;
            _userService = userService;
        }

        public IActionResult One(string id)
        {
            TagData? tagData = _tagService.GetTagByName(id);

            if (tagData is not null)
            {
                TagViewModel tagVM = new TagViewModel();
                tagVM.Tag = tagData;
                tagVM.Tweets = _tagService.GetTagTweets(id).OrderByDescending(t => t.CreatedAt).ToList();
                tagVM.Users = _userService.GetAllUsersWithFollows().Take(5).ToList();
                tagVM.Tags = _tagService.GetAllTags().Take(5).ToList();

                return View(tagVM);
            }

            return NotFound();
        }

        public IActionResult All()
        {
            List<TagData> tags = _tagService.GetAllTags().ToList();
            return View(tags);
        }
    }
}
