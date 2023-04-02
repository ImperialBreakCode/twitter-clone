using TwitterUni.Data.Entities;

namespace TwitterUni.Services.ModelData
{
    public class FollowData
    {
        public string TheFollowerId { get; set; }
        public User TheFollower { get; set; }
        public string IsFollowingId { get; set; }
        public User IsFollowing { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
