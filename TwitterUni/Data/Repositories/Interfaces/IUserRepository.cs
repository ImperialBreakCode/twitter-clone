
using TwitterUni.Data.Entities;

namespace TwitterUni.Data.Repositories.BaseRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        User? GetByUsername(string username);
        bool AddUserRetweet(string userId, Tweet tweet);
        bool RemoveUserRetweet(string userId, string tweetId);
        bool AddUserFollowing(string userId, string followingUserId);
        bool RemoveUserFollowing(string followerUserName, string followingUserName);
    }
}
