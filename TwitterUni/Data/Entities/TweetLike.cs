namespace TwitterUni.Data.Entities
{
    public class TweetLike
    {
        public int Id { get; set; }
        public Tweet Tweet { get; set; }
        public User LikeByUser { get; set; }
    }
}
