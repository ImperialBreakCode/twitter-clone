

namespace TwitterUni.Data.Entities
{
    public class TweetActivity
    {
        public int TweetActivityId { get; set; }
        public string Type { get; set; }
        public Tweet Tweet { get; set; }
        public User Doer { get; set; }
    }
}
