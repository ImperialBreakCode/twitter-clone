using TwitterUni.Services.ModelData;

namespace TwitterUni.Models.Tweet
{
    public class TweetViewModel
    {
        public TweetData Tweet { get; set; }
        public ICollection<TagData> Tags { get; set; }
    }
}
