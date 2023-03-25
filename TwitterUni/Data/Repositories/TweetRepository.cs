using TwitterUni.Data.Entities;
using TwitterUni.Data.Repositories.BaseRepositories;

namespace TwitterUni.Data.Repositories
{
    public class TweetRepository : Repository<Tweet>, ITweetRepository
    {
        public TweetRepository(TwitterDbContext context) : base(context)
        {
        }

        public void AddRetweet(string tweetId, User user)
        {
            var tweet = GetOne(tweetId);

            if (tweet is not null)
            {
                Retweet retweet = new Retweet() { CreatedAt = DateTime.UtcNow };
                tweet.Retweets.Add(retweet);
                user.Retweets.Add(retweet);
            }
        }

        public void RemoveRetweet(string tweetId, string userId)
        {
            var retweet = Context.UserRetweets.FirstOrDefault(r => r.TweetId == tweetId && r.RetweetedById == userId);

            if (retweet is not null)
            {
                Context.UserRetweets.Remove(retweet);
            }
        }
    }
}
