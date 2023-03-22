using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TwitterUni.Data.Entities;

namespace TwitterUni.Data;

public class TwitterDbContext : IdentityDbContext
{
    public TwitterDbContext(DbContextOptions<TwitterDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Tweet> Tweets { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<TweetActivity> TweetActivities { get; set; }

    // many to many mapping tables
    public DbSet<Follows> Follows { get; set; }
    public DbSet<TweetLike> TweetLikes { get; set; }
    public DbSet<Retweet> UserRetweets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-MR3OHEC\\SQLEXPRESS;Database=TwitterDb;Encrypt=False;Trusted_Connection=True;");
    }
}
