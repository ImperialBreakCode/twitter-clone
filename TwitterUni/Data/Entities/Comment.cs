using TwitterUni.Data.BaseEntities;

namespace TwitterUni.Data.Entities
{
    public class Comment : BaseEntity
    {
        public string TextContent { get; set; }
        public Tweet ParentTweet { get; set; }
        public User Author { get; set; }
        public ICollection<User> Likes { get; set; }
    }
}
