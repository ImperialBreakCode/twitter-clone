namespace TwitterUni.Data.Entities
{
    public class Retweet
    {
        public int RetweetId { get; set; }
        public Tweet Tweet { get; set; }
        public User RetweetedBy { get; set; }
    }
}
