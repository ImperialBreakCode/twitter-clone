using AutoMapper;
using TwitterUni.Data.Entities;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Services.Mapping
{
    public class MappingServiceDataProfile : Profile
    {
        public MappingServiceDataProfile()
        {
            // To data
            CreateMap<User, UserData>();
            CreateMap<Follow, FollowData>();
            CreateMap<Tweet, TweetData>();
            CreateMap<Retweet, RetweetData>();
            CreateMap<Tag, TagData>()
                .ForMember(dest => dest.TweetCount, opt => opt.MapFrom(src => src.Tweets.Count));

            // To entity
            CreateMap<UserData, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FollowersCollection, opt => opt.Ignore())
                .ForMember(dest => dest.FollowingsCollection, opt => opt.Ignore());

            CreateMap<TweetData, Tweet>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Author, opt => opt.Ignore())
                .ForMember(dest => dest.Retweets, opt => opt.Ignore())
                .ForMember(dest => dest.UserLikes, opt => opt.Ignore());

            CreateMap<TagData, Tag>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
