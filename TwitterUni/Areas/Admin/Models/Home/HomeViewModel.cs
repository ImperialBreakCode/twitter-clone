using TwitterUni.Services.ModelData;

namespace TwitterUni.Areas.Admin.Models.Home
{
    public class HomeViewModel
    {
        public ICollection<UserData> Users { get; set; }
        public int UserCount { get; set; }
        public int TweetCount { get; set; }
        public int TagCount { get; set; }
        public int CommentCount { get; set; }
    }
}
