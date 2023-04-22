using TwitterUni.Services.ModelData;

namespace TwitterUni.Models.User
{
    public class FollowsViewModel
    {
        public string UserName { get; set; }
        public ICollection<UserData> UserList { get; set; }
    }
}
