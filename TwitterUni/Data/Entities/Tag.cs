

namespace TwitterUni.Data.Entities
{
    public class Tag
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public ICollection<Tweet> Tweets { get; set; }
    }
}
