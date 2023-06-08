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
        public JsonResult DeleteComment(string commentId)
        {
            var comment = _commentService.GetComment(commentId);

            if (comment is null)
            {
                return new JsonResult(NotFound("Could not delete comment."));
            }

            if (comment.Author.UserName == User.Identity.Name)
            {
                _commentService.DeleteComment(commentId);
                return new JsonResult(Ok());
            }

            Response.StatusCode = (int)HttpStatusCode.Forbidden;
            return new JsonResult("Could not delete comment.");
        }

        [HttpPost]
        public JsonResult LikeComment(string commentId)
        {
            bool success = _commentService.LikeComment(commentId, User.Identity.Name);

            if (success)
            {
                return new JsonResult(Ok());
            }

            Response.StatusCode = 401;
            return new JsonResult(NotFound("Comment not found"));
        }

        [HttpDelete]
        public JsonResult UnikeComment(string commentId)
        {
            bool success = _commentService.UnlikeComment(commentId, User.Identity.Name);

            if (success)
            {
                return new JsonResult(Ok());
            }

            Response.StatusCode = 401;
            return new JsonResult(NotFound("Comment not found"));
        }
    }
}
