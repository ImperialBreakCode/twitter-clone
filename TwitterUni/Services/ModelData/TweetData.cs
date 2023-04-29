
namespace TwitterUni.Services.ModelData
{
    public class TweetData
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRetweet { get; set; } = false;
        public DateTime ProfileOrderingDate { get; set; }
        public string TextContent { get; set; }
        public string Image { get; set; }
        public string CommentsCount { get; set; }
        public UserData Author { get; set; }
        public ICollection<UserData> UserLikes { get; set; }
        public ICollection<RetweetData> Retweets { get; set; }
    }
}
