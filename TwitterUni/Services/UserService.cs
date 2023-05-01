using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TwitterUni.Data.Entities;
using TwitterUni.Data.UnitOfWork;
using TwitterUni.Infrastructure.Constants;
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

        public async Task<IdentityResult> CreateUser(UserData userData, string password)
        {
            User user = new User();
            _mapper.Map(userData, user);
            IdentityResult result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, RoleNames.User);
            }

            return result;
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

        public IEnumerable<UserData> GetAllUsersWithFollows()
        {
            var users = _unitOfWork.UserRepository.GetAll()
                .Include(u => u.FollowersCollection).ThenInclude(f => f.TheFollower)
                .Include(u => u.FollowingsCollection).ThenInclude(f => f.IsFollowing);

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

        public async Task<SignInResult?> SignInUser(string userName, string password)
        {
            var user = _unitOfWork.UserRepository.GetByUsername(userName);

            if (user is not null)
            {
                var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);
                
                return result;
            }

            return null;
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

            if (user is not null && !user.IsSet)
            {
                _mapper.Map(userData, user);
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, password);
                user.IsSet = true;
                
                _unitOfWork.Commit();
            }
        }

        public bool FollowUser(string followerId, string followingId)
        {
            bool isSuccess = _unitOfWork.UserRepository.AddUserFollowing(followerId, followingId);
            _unitOfWork.Commit();

            return isSuccess;
        }

        public bool UnfollowUser(string followerUserName, string followingUserName)
        {
            bool isFound = _unitOfWork.UserRepository.RemoveUserFollowing(followerUserName, followingUserName);
            _unitOfWork.Commit();

            return isFound;
        }

        public async Task<bool> CheckPassword(string userName, string password)
        {
            User? user = _unitOfWork.UserRepository.GetByUsername(userName);

            if (user is not null)
            {
                return await _userManager.CheckPasswordAsync(user, password);
            }

            return false;
        }

        public async Task<IdentityResult> DeleteUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            
            _unitOfWork.UserRepository.DeleteAllFollows(id);
            _unitOfWork.UserRepository.DeleteAllRetweets(id);
            
            DeleteUserTweets(id);
            DeleteUserComments(id);

            IdentityResult result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                _unitOfWork.Commit();
            }

            return result;
        }

        private void DeleteUserTweets(string userId)
        {
            IQueryable<Tweet> userTweets = _unitOfWork.TweetRepository.GetAll()
                .Include(t => t.Author)
                .Include(t => t.Retweets)
                .Include(t => t.Comments)
                .Where(t => t.Author.Id == userId);

            foreach (Tweet tweet in userTweets)
            {
                _unitOfWork.CommentRepository.DeleteRange(tweet.Comments.ToArray());
                _unitOfWork.TweetRepository.DeleteAllRetweets(tweet.Id);
            }
            _unitOfWork.TweetRepository.DeleteRange(userTweets.ToArray());
        }

        private void DeleteUserComments(string userId)
        {
            IQueryable<Comment> userComments = _unitOfWork.CommentRepository.GetAll()
                .Include(t => t.Author)
                .Where(t => t.Author.Id == userId);

            _unitOfWork.CommentRepository.DeleteRange(userComments.ToArray());
        }
    }
}
