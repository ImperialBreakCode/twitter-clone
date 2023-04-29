using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TwitterUni.Infrastructure.Constants;
using TwitterUni.Infrastructure.Filters;
using TwitterUni.Models.Comment;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Controllers
{
    [Authorize(Roles = RoleNames.User)]
    [SetupUserFilter]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public JsonResult CreateComment(CreateCommentModel createModel)
        {
            if (string.IsNullOrWhiteSpace(createModel.TweetId) && string.IsNullOrWhiteSpace(createModel.Text))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return new JsonResult("TweetId and text should not be empty");    
            }

            CommentData? commentData = new CommentData() { TextContent = createModel.Text };
            commentData = _commentService.CreateComment(commentData, User.Identity.Name, createModel.TweetId);

            if (commentData is not null)
            {
                createModel.Id = commentData.Id;
                return new JsonResult(createModel);
            }

            Response.StatusCode = (int)HttpStatusCode.NotFound;
            return new JsonResult(NotFound("User or tweet not found"));
        }

        [HttpDelete]
        public JsonResult DeleteComment()
        {
            return new JsonResult(Ok());
        }
    }
}
