using TwitterUni.Data.Entities;
using TwitterUni.Data.Repositories.BaseRepositories;

namespace TwitterUni.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(TwitterDbContext context) : base(context)
        {
        }

        public void AddUserFollowing(string userId, string followingUserId)
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
                //follower.FollowingsCollection.Add(follow);
                //following.FollowersCollection.Add(follow);
            }
        }

        public void RemoveUserFollowing(string userId, string followingUserId)
        {
            var follow = Context.Follows.FirstOrDefault(f => f.TheFollowerId == userId && f.IsFollowingId == followingUserId);

            if (follow is not null)
            {
                Context.Follows.Remove(follow);
            }
        }

        public void AddUserRetweet(string userId, Tweet tweet)
        {
            var user = GetOne(userId);

            if (user is not null)
            {
                Retweet retweet = new Retweet() { CreatedAt = DateTime.UtcNow };
                user.Retweets.Add(retweet);
                tweet.Retweets.Add(retweet);
            }
        }

        public void RemoveUserRetweet(string userId, string tweetId)
        {
            var retweet = Context.UserRetweets.FirstOrDefault(r => r.TweetId == tweetId && r.RetweetedById == userId);

            if (retweet is not null)
            {
                Context.UserRetweets.Remove(retweet);
            }
        }

        public User? GetByUsername(string username)
        {
            var user = Context.Users.FirstOrDefault(u => u.UserName == username);
            return user;
        }
    }
}
