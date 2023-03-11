namespace TwitterUni.Data.Entities
{
    public class Follows
    {
        public int Id { get; set; }
        public User TheFollower { get; set; }
        public User IsFollowing { get; set; }
    }
}
