using AutoMapper;
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
        private readonly Mapper _mapper;

        public UserService(
            UserManager<User> userManager, 
            SignInManager<User> signInManager,
            IUnitOfWork unitOfWork,
            Mapper mapper
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        public async Task<bool> CreateUser(UserData userData, string password)
        {
            User user = new User();
            _mapper.Map(userData, user);
            IdentityResult result = await _userManager.CreateAsync(user, password);

            return result.Succeeded;
        }

        public async Task DeleteUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public IEnumerable<UserData> GetAllUsers()
        {
            var users = _unitOfWork.UserRepository.GetAll();
            List<UserData> result = new List<UserData>();

            foreach (var user in users)
            {
                result.Add(_mapper.Map<UserData>(user));
            }

            return result;
        }

        public UserData? GetUserById(string id)
        {
            User? user = _unitOfWork.UserRepository.GetOne(id);
            UserData? userData = null;

            if (user is not null)
            {
                userData = _mapper.Map<UserData>(user);
            }

            return userData;
        }

        public UserData? GetUserByUserName(string userName)
        {
            User? user = _unitOfWork.UserRepository.GetByUsername(userName);
            UserData? userData = null;

            if (user is not null)
            {
                userData = _mapper.Map<UserData>(user);
            }

            return userData;
        }

        public async Task<bool> SignInUser(string userName, string password)
        {
            var user = _unitOfWork.UserRepository.GetByUsername(userName);

            if (user is not null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);
                
                return result.Succeeded;
            }

            return false;
        }

        public async Task SignOutUser()
        {
            await _signInManager.SignOutAsync();
        }

        public void UpdateUser(UserData userData)
        {
            User? user = _unitOfWork.UserRepository.GetOne(userData.Id);

            if (user is not null)
            {
                _mapper.Map(userData, user);
                _unitOfWork.Commit();
            }
        }

        public void CompleteUserSetup(UserData userData, string password)
        {
            User? user = _unitOfWork.UserRepository.GetOne(userData.Id);

            if (user is not null)
            {
                _mapper.Map(userData, user);
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, password);
                user.IsSet = true;
                
                _unitOfWork.Commit();
            }
        }
    }
}
