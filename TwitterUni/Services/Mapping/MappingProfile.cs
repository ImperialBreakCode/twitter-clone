using AutoMapper;
using TwitterUni.Data.Entities;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // To data
            CreateMap<User, UserData>();
            CreateMap<Follow, FollowData>();

            // To entity
            CreateMap<UserData, User>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.FollowersCollection, opt => opt.Ignore())
                .ForMember(dest => dest.FollowingsCollection, opt => opt.Ignore());
        }
    }
}
