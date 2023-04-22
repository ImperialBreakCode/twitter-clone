using TwitterUni.Data.Entities;

namespace TwitterUni.Services.ModelData
{
    public class FollowData
    {
        public string TheFollowerId { get; set; }
        public UserData TheFollower { get; set; }
        public string IsFollowingId { get; set; }
        public UserData IsFollowing { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
