﻿using TwitterUni.Services.ModelData;

namespace TwitterUni.Services.Interfaces
{
    public interface ITweetService
    {
        public void CreateTweet(TweetData tweet, string authorUserName);
        public void UpdateTweet(TweetData tweet);
        public void DeleteTweet(string tweetId);
        public TweetData? GetTweet(string tweetId);
        public ICollection<TweetData> GetAllTweets();
        public ICollection<TweetData> GetTweetsByUser(string userName);
        public bool LikeTweet(string userName, string tweetId);
        public bool UnlikeTweet(string userName, string tweetId);
    }
}
