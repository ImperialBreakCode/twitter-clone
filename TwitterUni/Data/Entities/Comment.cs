

namespace TwitterUni.Data.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string TextContent { get; set; }
        public DateTime PublishDate { get; set; }
        public Tweet ParentTweet { get; set; }
        public User Author { get; set; }
        public ICollection<User> Likes { get; set; }
    }
}
