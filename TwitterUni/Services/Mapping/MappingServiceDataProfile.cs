﻿using AutoMapper;
using Microsoft.Build.Framework;
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
            CreateMap<Tweet, TweetData>()
                .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.UserLikes.Count));
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
                .ForMember(dest => dest.Author, opt => opt.Ignore());

            CreateMap<TagData, Tag>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
