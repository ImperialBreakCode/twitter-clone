using TwitterUni.Data.BaseEntities;

namespace TwitterUni.Data.Entities
{
    public class Tag : BaseEntity
    {
        public string TagName { get; set; }
        public ICollection<Tweet> Tweets { get; set; }
    }
}
