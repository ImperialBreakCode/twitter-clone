using TwitterUni.Data.BaseEntities;

namespace TwitterUni.Data.Entities
{
    // join table
    public class Follows : BaseEntity
    {
        public User TheFollower { get; set; }
        public User IsFollowing { get; set; }
    }
}
