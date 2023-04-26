using TwitterUni.Services.ModelData;

namespace TwitterUni.Models.Tag
{
    public class TagViewModel
    {
        public TagData Tag { get; set; }
        public ICollection<TweetData> Tweets { get; set; }
        public ICollection<UserData> Users { get; set; }
        public ICollection<TagData> Tags { get; set; }
    }
}
