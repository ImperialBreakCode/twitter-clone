using TwitterUni.Data.BaseEntities;

namespace TwitterUni.Data.Entities
{
    public class Retweet : BaseEntity
    {
        public Tweet Tweet { get; set; }
        public User RetweetedBy { get; set; }
    }
}
