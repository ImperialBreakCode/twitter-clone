using TwitterUni.Data.Entities;

namespace TwitterUni.Services.ModelData
{
    public class RetweetData
    {
        public string TweetId { get; set; }
        public TweetData Tweet { get; set; }
        public string RetweetedById { get; set; }
        public UserData RetweetedBy { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
