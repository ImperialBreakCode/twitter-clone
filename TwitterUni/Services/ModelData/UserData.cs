using TwitterUni.Data.Entities;

namespace TwitterUni.Services.ModelData
{
    public class UserData
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePic { get; set; }
        public string BackgroundPhoto { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? BirthDate { get; set; }
        public ICollection<Follow> FollowersCollection { get; set; }
        public ICollection<Follow> FollowingsCollection { get; set; }
    }
}
