using TwitterUni.Services.ModelData;

namespace TwitterUni.Models.Tweet
{
    public class TweetViewModel
    {
        public UserData CurrentUser { get; set; }
        public TweetData Tweet { get; set; }
        public ICollection<TagData> Tags { get; set; }
        public ICollection<CommentData> Comments { get; set; }
    }
}
