using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TwitterUni.Data.Entities;
using TwitterUni.Data.UnitOfWork;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Services
{
    public class TweetService : ITweetService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mapper _mapper;

        public TweetService(IUnitOfWork unitOfWork, Mapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void CreateTweet(TweetData tweetData, string authorUserName)
        {
            Tweet tweet = new Tweet();
            _mapper.Map(tweetData, tweet);

            User? user = _unitOfWork.UserRepository.GetByUsername(authorUserName);

            if (user is not null)
            {
                //_unitOfWork.TweetRepository.CreateOne(tweet);
                user.Tweets.Add(tweet);
                _unitOfWork.Commit();
            }
        }

        public void DeleteTweet(string tweetId)
        {
            _unitOfWork.TweetRepository.DeleteOne(tweetId);
            _unitOfWork.Commit();
        }

        public ICollection<TweetData> GetAllTweets()
        {
            IQueryable<Tweet> tweets = _unitOfWork.TweetRepository.GetAll().Include(t => t.Author);
            List<TweetData> tweetDatas = new List<TweetData>();

            foreach (var tweet in tweets)
            {
                tweetDatas.Add(_mapper.Map<TweetData>(tweet));
            }

            return tweetDatas;
        }

        public TweetData? GetTweet(string tweetId)
        {
            Tweet? tweet = _unitOfWork.TweetRepository.GetOne(tweetId);
            TweetData? tweetData = null;

            if (tweet is not null)
            {
                tweetData = _mapper.Map<TweetData>(tweet);
            }
            return tweetData;
        }

        public ICollection<TweetData> GetTweetsByUser(string userName)
        {
            User? user = _unitOfWork.UserRepository.GetByUsername(userName);
            ICollection<TweetData> tweetsData = new List<TweetData>();

            if (user is not null)
            {
                foreach (var tweet in user.Tweets)
                {
                    tweetsData.Add(_mapper.Map<TweetData>(tweet));
                }
            }

            return tweetsData;
        }

        public bool LikeTweet(string userName, string tweetId)
        {
            User? user = _unitOfWork.UserRepository.GetByUsername(userName);
            Tweet? tweet = _unitOfWork.TweetRepository.GetOne(tweetId);

            if (user is not null && tweet is not null)
            {
                user.LikedTweets.Add(tweet);
                //tweet.Author = user;
                _unitOfWork.Commit();
            }

            return user is not null && tweet is not null;
        }

        public bool UnlikeTweet(string userName, string tweetId)
        {
            User? user = _unitOfWork.UserRepository.GetByUsername(userName);
            Tweet? tweet = _unitOfWork.TweetRepository.GetOne(tweetId);

            if (user is not null && tweet is not null)
            {
                user.LikedTweets.Remove(tweet);
                _unitOfWork.Commit();
            }

            return user is not null && tweet is not null;
        }

        public void UpdateTweet(TweetData tweetData)
        {
            Tweet? tweet = _unitOfWork.TweetRepository.GetOne(tweetData.Id);

            if (tweet is not null)
            {
                _mapper.Map(tweetData, tweet);
                _unitOfWork.Commit();
            }
        }
    }
}
