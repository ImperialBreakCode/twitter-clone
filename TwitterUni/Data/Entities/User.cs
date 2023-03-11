﻿namespace TwitterUni.Data.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePic { get; set; }
        public string BackgroundPhoto { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Tweet> Tweets { get; set; }
        public ICollection<Tweet> Retweets { get; set; }
        public ICollection<Tweet> LikedTweets { get; set; }
        public ICollection<User> Followers { get; set; }
        public ICollection<User> Following { get; set; }
    }
}