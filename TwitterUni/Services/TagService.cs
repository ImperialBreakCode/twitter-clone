using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TwitterUni.Data.Entities;
using TwitterUni.Data.UnitOfWork;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Services
{
    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mapper _mapper;

        public TagService(IUnitOfWork unitOfWork, Mapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public void AddTagsToTweet(string tweetId, ICollection<string> tagNames)
        {
            Tweet? tweet = _unitOfWork.TweetRepository.GetOne(tweetId);

            foreach (string tagName in tagNames)
            {
                Tag? tag = _unitOfWork.TagRepository.GetTagByName(tagName);

                if (tweet is not null)
                {
                    if (tag == null)
                    {
                        tag = new Tag() { TagName = tagName };
                        _unitOfWork.TagRepository.CreateOne(tag);
                    }

                    tag.Tweets.Add(tweet);
                }
            }

            _unitOfWork.Commit();
        }

        public void CreateTag(TagData tagData)
        {
            Tag tag = _mapper.Map<Tag>(tagData);
            _unitOfWork.TagRepository.CreateOne(tag);
            _unitOfWork.Commit();
        }

        public void DeleteEmptyTags()
        {
            List<string> emptyTagsIds = _unitOfWork.TagRepository.GetAll()
                .Include(t => t.Tweets)
                .Where(t => t.Tweets.Count == 0).Select(t => t.Id)
                .ToList();

            foreach (string tagId in emptyTagsIds)
            {
                _unitOfWork.TagRepository.DeleteOne(tagId);
            }

            _unitOfWork.Commit();
        }

        public void DeleteTag(string id)
        {
            _unitOfWork.TagRepository.DeleteOne(id);
            _unitOfWork.Commit();
        }

        public ICollection<TagData> GetAllTags()
        {
            List<TagData> tagDatas = new List<TagData>();
            IQueryable<Tag> tags = _unitOfWork.TagRepository.GetAll()
                .Include(t => t.Tweets)
                .OrderByDescending(t => t.Tweets.Count);

            foreach (var tag in tags)
            {
                tagDatas.Add(_mapper.Map<TagData>(tag));
            }

            return tagDatas;
        }

        public TagData? GetTagById(string id)
        {
            Tag? tag = _unitOfWork.TagRepository.GetOne(id);
            TagData? tagData = null;

            if (tag is not null)
            {
                tagData = _mapper.Map<TagData>(tag);
            }

            return tagData;
        }

        public TagData? GetTagByName(string name)
        {
            Tag? tag = _unitOfWork.TagRepository.GetTagByName(name);
            TagData? tagData = null;

            if (tag is not null)
            {
                tagData = _mapper.Map<TagData>(tag);
            }

            return tagData;
        }

        public ICollection<TweetData> GetTagTweets(string name)
        {
            Tag? tag = _unitOfWork.TagRepository.GetTagByName(name);
            List<TweetData> tweetDatas = new List<TweetData>();

            if (tag is not null)
            {
                foreach (var tweet in tag.Tweets)
                {
                    tweetDatas.Add(_mapper.Map<TweetData>(tweet));
                }
            }

            return tweetDatas;
        }

        public ICollection<TagData> GetTweetTags(string tweetId)
        {
            Tweet? tweet = _unitOfWork.TweetRepository.GetOne(tweetId);
            List<TagData> tagDatas = new List<TagData>();

            if (tweet is not null)
            {
                foreach (var tag in tweet.Tags)
                {
                    tagDatas.Add(_mapper.Map<TagData>(tag));
                }
            }

            return tagDatas;
        }

        public void RemoveTweetFromTag(string tweetId, string tagName)
        {
            Tag? tag = _unitOfWork.TagRepository.GetTagByName(tagName);
            Tweet? tweet = _unitOfWork.TweetRepository.GetOne(tweetId);

            if (tag is not null && tweet is not null)
            {
                tag.Tweets.Remove(tweet);

                if (tag.Tweets.Count == 0)
                {
                    _unitOfWork.TagRepository.DeleteOne(tag.Id);
                }

                _unitOfWork.Commit();
            }
        }

        public void UpdateTag(TagData tagData)
        {
            Tag? tag = _unitOfWork.TagRepository.GetOne(tagData.Id);

            if (tag is not null)
            {
                _mapper.Map(tagData, tag);
                _unitOfWork.Commit();
            }
        }
    }
}
