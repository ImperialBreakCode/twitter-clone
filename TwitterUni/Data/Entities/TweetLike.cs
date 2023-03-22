using TwitterUni.Data.BaseEntities;

namespace TwitterUni.Data.Entities
{
    // join table
    public class TweetLike : BaseEntity
    {
        public Tweet Tweet { get; set; }
        public User LikeByUser { get; set; }
    }
}
