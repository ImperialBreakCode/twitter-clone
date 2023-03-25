
using TwitterUni.Data.Entities;

namespace TwitterUni.Data.Repositories.BaseRepositories
{
    public interface IUserRepository : IRepository<User>
    {
        void AddUserRetweet(string userId, Tweet tweet);
        void RemoveUserRetweet(string userId, string tweetId);
        void AddUserFollowing(string userId, string followingUserId);
        void RemoveUserFollowing(string userId, string followingUserId);
    }
}
