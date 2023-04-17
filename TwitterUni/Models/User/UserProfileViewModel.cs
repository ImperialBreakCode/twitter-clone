using TwitterUni.Services.ModelData;

namespace TwitterUni.Models.User
{
    public class UserProfileViewModel
    {
        public UserData User { get; set; }
        public ICollection<UserData> OtherUsers { get; set; }
    }
}
