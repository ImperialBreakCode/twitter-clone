using TwitterUni.Services.ModelData;

namespace TwitterUni.Services.Interfaces
{
    public interface ICommentService
    {
        CommentData? CreateComment(CommentData commentData, string userName, string tweetId);
        void UpdateComment(CommentData commentData);
        void DeleteComment(string commentId);
        CommentData? GetComment(string commentId);
        ICollection<CommentData> GetTweetComments(string tweetId);
    }
}
