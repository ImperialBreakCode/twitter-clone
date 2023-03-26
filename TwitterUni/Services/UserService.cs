using Microsoft.AspNetCore.Identity;
using TwitterUni.Data.Entities;
using TwitterUni.Data.UnitOfWork;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<User> userManager, SignInManager<User> signInManager,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public UserManager<User> UserManager { get => _userManager; }

        public SignInManager<User> SignInManager { get => _signInManager; }

        public async Task CreateUser(User user)
        {
            await UserManager.CreateAsync(user, "abV12345_");
        }

        public async Task DeleteUser(string id)
        {
            User user = await UserManager.FindByIdAsync(id);
            await UserManager.DeleteAsync(user);
        }

        public IEnumerable<UserData> GetAllUsers()
        {
            var users = _unitOfWork.UserRepository.GetAll();
            List<UserData> result = new List<UserData>();

            foreach (var user in users)
            {
                result.Add(new UserData()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName
                });
            }

            return result;
        }

        public UserData GetUserById(string id)
        {
            User? user = _unitOfWork.UserRepository.GetOne(id);
            UserData userData = new UserData();

            if (user is not null)
            {
                userData.FirstName = user.FirstName;
                userData.LastName = user.LastName;
                userData.Id = user.Id;
            }

            return userData;
        }

        public UserData GetUserByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        // testing only
        public async Task SignInUser(string userName)
        {
            var user = _unitOfWork.UserRepository.GetByUsername(userName);

            if (user is not null)
            {
                var result = await SignInManager.PasswordSignInAsync(user.UserName, "abV12345_", false, false);
                if (result.Succeeded)
                {
                    Console.WriteLine("Seccess");
                }
            }
        }

        // testing only
        public async Task SignOutUser()
        {
            await SignInManager.SignOutAsync();
        }

        public void UpdateUser(UserData userData)
        {
            User? user = _unitOfWork.UserRepository.GetOne(userData.Id);

            if (user is not null)
            {
                user.Id = userData.Id;
                user.FirstName = userData.FirstName;
                user.LastName = userData.LastName;

                _unitOfWork.Commit();
            }
        }
    }
}
