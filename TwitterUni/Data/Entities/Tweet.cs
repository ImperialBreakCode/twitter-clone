using System.ComponentModel.DataAnnotations;
using TwitterUni.Data.BaseEntities;

namespace TwitterUni.Data.Entities
{
    public class Tweet : BaseEntity
    {
        [Required]
        public string TextContent { get; set; }
        public string Image { get; set; }
        public User Author { get; set; }
        public ICollection<User> UserLikes { get; set; }
        public ICollection<Retweet> Retweets { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Tag> Tags { get; set; }
        public ICollection<TweetActivity> Activities { get; set; }
    }
}
