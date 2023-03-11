

namespace TwitterUni.Data.Entities
{
    public class Tweet
    {
        public int TweetId { get; set; }
        public string TextContent { get; set; }
        public string Image { get; set; }
        public int Retweets { get; set; }
        public DateTime PublishDate { get; set; }
        public User Author { get; set; }
        public ICollection<User> LikedBy { get; set; }
        public ICollection<Tweet> RetweetedBy { get; set; }
        public ICollection<Comments> Comments { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<TweetActivity> Activities { get; set; }
    }
}
