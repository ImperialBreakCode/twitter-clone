using TwitterUni.Data.BaseEntities;

namespace TwitterUni.Data.Entities
{
    public class TweetActivity : BaseEntity
    {
        public string Type { get; set; }
        public Tweet Tweet { get; set; }
        public User Doer { get; set; }
    }
}
