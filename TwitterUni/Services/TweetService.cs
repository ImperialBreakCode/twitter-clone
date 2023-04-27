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

        public TweetData? CreateTweet(TweetData tweetData, string authorUserName)
        {
            Tweet tweet = new Tweet();
            _mapper.Map(tweetData, tweet);

            User? user = _unitOfWork.UserRepository.GetByUsername(authorUserName);

            if (user is not null)
            {
                user.Tweets.Add(tweet);
                _unitOfWork.Commit();

                return _mapper.Map<TweetData>(tweet);
            }

            return null;
        }

        public void DeleteTweet(string tweetId)
        {
            _unitOfWork.TweetRepository.DeleteOne(tweetId);
            _unitOfWork.Commit();
        }

        public ICollection<TweetData> GetAllTweets()
        {
            IQueryable<Tweet> tweets = _unitOfWork.TweetRepository.GetAll()
                .Include(t => t.Author)
                .Include(t => t.UserLikes)
                .Include(t => t.Retweets).ThenInclude(r => r.RetweetedBy)
                .OrderBy(t => t.CreatedAt);

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

        public ICollection<RetweetData> GetRetweetsByUser(string userName)
        {
            List<RetweetData> retweetDatas = new List<RetweetData>();

            var user = _unitOfWork.UserRepository.GetAll()
                .Include(u => u.Retweets).ThenInclude(r => r.Tweet.Author)
                .Include(u => u.Retweets).ThenInclude(r => r.Tweet.UserLikes)
                .Include(u => u.Retweets).ThenInclude(r => r.Tweet.Retweets).ThenInclude(r => r.RetweetedBy)
                .FirstOrDefault(u => u.UserName == userName);

            if (user is not null)
            {
                var retweets = user.Retweets;

                foreach (Retweet retweet in retweets)
                {
                    retweetDatas.Add(_mapper.Map<RetweetData>(retweet));
                }
            }

            return retweetDatas;
        }

        public bool LikeTweet(string userName, string tweetId)
        {
            User? user = _unitOfWork.UserRepository.GetByUsername(userName);
            Tweet? tweet = _unitOfWork.TweetRepository.GetOne(tweetId);

            if (user is not null && tweet is not null)
            {
                user.LikedTweets.Add(tweet);
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

        public bool CreateRetweet(string userName, string tweetId)
        {
            bool result = false;
            User? user = _unitOfWork.UserRepository.GetByUsername(userName);

            if (user is not null)
            {
                result = _unitOfWork.TweetRepository.AddRetweet(tweetId, user);
            }

            if (result)
            {
                _unitOfWork.Commit();
            }

            return result;
        }

        public bool DeleteRetweet(string userName, string tweetId)
        {
            bool result = false;
            User? user = _unitOfWork.UserRepository.GetByUsername(userName);

            if (user is not null)
            {
                result = _unitOfWork.TweetRepository.RemoveRetweet(tweetId, user.Id);
            }

            if (result)
            {
                _unitOfWork.Commit();
            }

            return result;
        }
    }
}
