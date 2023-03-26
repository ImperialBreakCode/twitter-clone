using Microsoft.AspNetCore.Identity;
using TwitterUni.Data.Entities;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Services.Interfaces
{
    public interface IUserService
    {
        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }

        Task CreateUser(User user);
        UserData GetUserById(string id);
        UserData GetUserByUserName(string userName);
        IEnumerable<UserData> GetAllUsers();
        void UpdateUser(UserData user);
        Task DeleteUser(string id);
        Task SignInUser(string userName);
        Task SignOutUser();
    }
}
