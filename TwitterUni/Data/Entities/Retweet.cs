using TwitterUni.Data.BaseEntities;

namespace TwitterUni.Data.Entities
{
    public class Retweet : IEntityCreationInfo
    {
        public string TweetId { get; set; }
        public Tweet Tweet { get; set; }
        public string RetweetedById { get; set; }
        public User RetweetedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
