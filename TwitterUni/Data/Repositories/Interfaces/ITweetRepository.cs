using TwitterUni.Data.Entities;

namespace TwitterUni.Data.Repositories.BaseRepositories
{
    public interface ITweetRepository : IRepository<Tweet>
    {
        bool AddRetweet(string tweetId, User user);
        bool RemoveRetweet(string tweetId, string userId);
    }
}
