using Microsoft.EntityFrameworkCore;
using TwitterUni.Data.Entities;
using TwitterUni.Data.Repositories.Interfaces;

namespace TwitterUni.Data.Repositories
{
    public class TweetRepository : Repository<Tweet>, ITweetRepository
    {
        public TweetRepository(TwitterDbContext context) : base(context)
        {
        }

        public override Tweet? GetOne(string id)
        {
            return Context.Tweets
                .Include(c => c.Comments)
                .Include(t => t.Author)
                .Include(t => t.Tags)
                .Include(t => t.UserLikes)
                .Include(t => t.Retweets).ThenInclude(r => r.RetweetedBy)
                .FirstOrDefault(t => t.Id == id);
        }

        public bool AddRetweet(string tweetId, User user)
        {
            var tweet = GetOne(tweetId);

            if (tweet is not null)
            {
                Retweet retweet = new Retweet()
                {
                    CreatedAt = DateTime.UtcNow,
                    RetweetedBy = user,
                    Tweet = tweet
                };

                Context.UserRetweets.Add(retweet);
            }

            return tweet is not null;
        }

        public bool RemoveRetweet(string tweetId, string userId)
        {
            var retweet = Context.UserRetweets.FirstOrDefault(r => r.TweetId == tweetId && r.RetweetedById == userId);

            if (retweet is not null)
            {
                Context.UserRetweets.Remove(retweet);
            }

            return retweet is not null;
        }

        public void DeleteAllRetweets(string tweetId)
        {
            IQueryable<Retweet> retweets = Context.UserRetweets.Where(r => r.TweetId == tweetId);
            Context.UserRetweets.RemoveRange(retweets);
        }
    }
}
