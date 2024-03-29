﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TwitterUni.Data.Entities;

namespace TwitterUni.Data;

public class TwitterDbContext : IdentityDbContext<User>
{
    public TwitterDbContext(DbContextOptions<TwitterDbContext> options)
        : base(options)
    {
    }

    public DbSet<AppSettings> AppSettings { get; set; }

    public override DbSet<User> Users { get; set; }
    public DbSet<Tweet> Tweets { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Tag> Tags { get; set; }

    // many to many mapping tables
    public DbSet<Follow> Follows { get; set; }
    public DbSet<Retweet> UserRetweets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        EntitiesConfig.Configure(builder);
    }
}
