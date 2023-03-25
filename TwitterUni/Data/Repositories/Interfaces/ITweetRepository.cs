using TwitterUni.Data.Entities;

namespace TwitterUni.Data.Repositories.BaseRepositories
{
    public interface ITweetRepository : IRepository<Tweet>
    {
        void AddRetweet(string tweetId, User user);
        void RemoveRetweet(string tweetId, string userId);
    }
}
