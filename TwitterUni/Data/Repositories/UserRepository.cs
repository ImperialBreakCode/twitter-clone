using Microsoft.EntityFrameworkCore;
using TwitterUni.Data.Entities;
using TwitterUni.Data.Repositories.Interfaces;

namespace TwitterUni.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(TwitterDbContext context) : base(context)
        {
        }

        public bool AddUserFollowing(string userId, string followingUserId)
        {
            var follower = GetOne(userId);
            var following = GetOne(followingUserId);

            if (follower is not null && following is not null)
            {
                Follow follow = new Follow() 
                { 
                    CreatedAt = DateTime.UtcNow,
                    TheFollower = follower,
                    IsFollowing = following
                };

                Context.Add(follow);
            }

            return follower is not null && following is not null;
        }

        public bool RemoveUserFollowing(string followerUserName, string followingUserName)
        {
            var follow = Context.Follows
                .FirstOrDefault(f => f.TheFollower.UserName == followerUserName && 
                    f.IsFollowing.UserName == followingUserName);

            if (follow is not null)
            {
                Context.Follows.Remove(follow);
            }

            return follow is not null;
        }

        public bool AddUserRetweet(string userId, Tweet tweet)
        {
            var user = GetOne(userId);

            if (user is not null)
            {
                Retweet retweet = new Retweet() 
                { 
                    CreatedAt = DateTime.UtcNow,
                    RetweetedBy = user,
                    Tweet = tweet
                };

                Context.UserRetweets.Add(retweet);
            }

            return user is not null;
        }

        public bool RemoveUserRetweet(string userId, string tweetId)
        {
            var retweet = Context.UserRetweets.FirstOrDefault(r => r.TweetId == tweetId && r.RetweetedById == userId);

            if (retweet is not null)
            {
                Context.UserRetweets.Remove(retweet);
            }

            return retweet is not null;
        }

        public User? GetByUsername(string username)
        {
            var user = Context.Users
                .Include(u => u.FollowersCollection)
                .ThenInclude(f => f.TheFollower)
                .Include(u => u.FollowingsCollection)
                .ThenInclude(f => f.IsFollowing)
                .Include(f => f.Tweets)
                .FirstOrDefault(u => u.UserName == username);

            return user;
        }

        public override User? GetOne(string id)
        {
            var user = Context.Users
                .Include(u => u.FollowersCollection)
                .ThenInclude(f => f.TheFollower)
                .Include(u => u.FollowingsCollection)
                .ThenInclude(f => f.IsFollowing)
                .Include(f => f.Tweets)
                .FirstOrDefault(u => u.Id == id);

            return user;
        }
    }
}
