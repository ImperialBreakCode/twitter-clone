using Microsoft.EntityFrameworkCore;
using TwitterUni.Data.Entities;

namespace TwitterUni.Data
{
    public static class EntitiesConfig
    {
        public static void Configure(ModelBuilder builder)
        {
            // User
            builder.Entity<User>()
                .HasIndex(u => u.UserName).IsUnique();

            // Tag
            builder.Entity<Tag>()
                .HasIndex(t => t.TagName).IsUnique();

            // follows mapping entity
            builder.Entity<Follow>()
                .HasKey(e => new { e.TheFollowerId, e.IsFollowingId });

            builder.Entity<Follow>()
                .HasOne(f => f.TheFollower)
                .WithMany(u => u.FollowingsCollection).OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Follow>()
                .HasOne(f => f.IsFollowing)
                .WithMany(u => u.FollowersCollection).OnDelete(DeleteBehavior.NoAction);

            // retweet mapping entity
            builder.Entity<Retweet>()
                .HasKey(e => new { e.TweetId, e.RetweetedById });
        }
    }
}
