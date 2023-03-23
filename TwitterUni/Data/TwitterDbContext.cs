﻿using Microsoft.AspNetCore.Identity;
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
    public DbSet<Follow> Follows { get; set; }
    public DbSet<Retweet> UserRetweets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=DESKTOP-MR3OHEC\\SQLEXPRESS;Database=TwitterDb;Encrypt=False;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

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
