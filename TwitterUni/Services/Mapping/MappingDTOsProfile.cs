using AutoMapper;
using TwitterUni.Data.Entities;
using TwitterUni.Services.ApiFetching.DTOs;

namespace TwitterUni.Services.Mapping
{
    public class MappingDTOsProfile : Profile
    {
        public MappingDTOsProfile()
        {
            CreateMap<UserDTO, User>()
                .ForMember(dest => dest.FirstName, 
                            opt => opt.MapFrom(src => src.Name.Split(' ', StringSplitOptions.None)[0]))
                .ForMember(dest => dest.LastName,
                            opt => opt.MapFrom(src => src.Name.Split(' ', StringSplitOptions.None)[1]));

            CreateMap<UserPostDTO, Tweet>();
        }
    }
}
