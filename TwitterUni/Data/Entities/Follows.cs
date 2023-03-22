using TwitterUni.Data.BaseEntities;

namespace TwitterUni.Data.Entities
{
    public class Follows : IEntityCreationInfo
    {
        public string TheFollowerId { get; set; }
        public User TheFollower { get; set; }
        public string IsFollowingId { get; set; }
        public User IsFollowing { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
