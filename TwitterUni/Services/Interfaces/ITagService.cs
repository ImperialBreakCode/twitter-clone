using TwitterUni.Services.ModelData;

namespace TwitterUni.Services.Interfaces
{
    public interface ITagService
    {
        TagData? GetTagById(string id);
        TagData? GetTagByName(string name);
        ICollection<TweetData> GetTagTweets(string name);
        ICollection<TagData> GetAllTags();
        void CreateTag(TagData tag);
        void UpdateTag(TagData tag);
        void DeleteTag(string id);
        void DeleteEmptyTags();
        void AddTagsToTweet(string tweetId, ICollection<string> tagNames);
        void RemoveTweetFromTag(string tweetId, string tagName);
        ICollection<TagData> GetTweetTags(string tweetId);
    }
}
