﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TwitterUni.Data.BaseEntities;

namespace TwitterUni.Data.Entities
{
    public class User : IdentityUser, IEntityCreationInfo, IEntityId
    {
        public User()
        {
            CreatedAt = DateTime.UtcNow;
            FollowersCollection = new List<Follow>();
            FollowingsCollection = new List<Follow>();
        }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string? ProfilePic { get; set; }
        public string? BackgroundPhoto { get; set; }
        public string? Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? BirthDate { get; set; }

        [Required]
        public bool IsSet { get; set; }

        [InverseProperty(nameof(Tweet.Author))]
        public ICollection<Tweet> Tweets { get; set; }
        public ICollection<Retweet> Retweets { get; set; }
        public ICollection<Tweet> LikedTweets { get; set; }

        [InverseProperty(nameof(Comment.Author))]
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Comment> LikedComments { get; set; }

        [InverseProperty(nameof(Follow.IsFollowing))]
        public ICollection<Follow> FollowersCollection { get; set; }

        [InverseProperty(nameof(Follow.TheFollower))]
        public ICollection<Follow> FollowingsCollection { get; set; }
    }
}