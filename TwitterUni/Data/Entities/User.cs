﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TwitterUni.Data.BaseEntities;

namespace TwitterUni.Data.Entities
{
    public class User : IdentityUser, IEntityCreationInfo, IEntityId
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public string ProfilePic { get; set; }
        public string BackgroundPhoto { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime BirthDate { get; set; }

        [InverseProperty(nameof(Tweet.Author))]
        public ICollection<Tweet> Tweets { get; set; }
        public ICollection<Retweet> Retweets { get; set; }
        public ICollection<Tweet> LikedTweets { get; set; }

        [InverseProperty(nameof(Comment.Author))]
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Comment> LikedComments { get; set; }

        [InverseProperty(nameof(Follows.IsFollowing))]
        public ICollection<Follows> FollowersCollection { get; set; }

        [InverseProperty(nameof(Follows.TheFollower))]
        public ICollection<Follows> FollowingsCollection { get; set; }
    }
}