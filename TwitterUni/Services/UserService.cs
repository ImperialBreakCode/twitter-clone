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
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<User> userManager, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public UserManager<User> UserManager { get => _userManager; }

        public async Task CreateUser(User user)
        {
            await UserManager.CreateAsync(user, "0000");
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
