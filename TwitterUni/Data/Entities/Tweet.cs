

namespace TwitterUni.Data.Entities
{
    public class Tweet
    {
        public int TweetId { get; set; }
        public string TextContent { get; set; }
        public string Image { get; set; }
        public DateTime PublishDate { get; set; }
        public User Author { get; set; }
        public ICollection<TweetLike> Likes { get; set; }
        public ICollection<Retweet> Retweets { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<TweetActivity> Activities { get; set; }
    }
}
