
using TwitterUni.Data.Entities;

namespace TwitterUni.Data.Repositories.BaseRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetByUsername(string username);
        void AddUserRetweet(string userId, Tweet tweet);
        void RemoveUserRetweet(string userId, string tweetId);
        void AddUserFollowing(string userId, string followingUserId);
        bool RemoveUserFollowing(string followerUserName, string followingUserName);
    }
}
