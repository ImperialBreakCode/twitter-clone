using AutoMapper;
using TwitterUni.Areas.Account.Models.Auth;
using TwitterUni.Areas.Account.Models.Settings;
using TwitterUni.Models.User;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Services.Mapping
{
    public class MappingViewModelProfile : Profile
    {
        public MappingViewModelProfile()
        {
            // to data
            CreateMap<ChangeUserInfoViewModel, UserData>();
            CreateMap<RegisterViewModel, UserData>();
            CreateMap<EditUserViewModel, UserData>()
                .ForMember(dest => dest.BackgroundPhoto, opt => opt.Ignore())
                .ForMember(dest => dest.ProfilePic, opt => opt.Ignore());

            // to viewModel
            CreateMap<UserData, ChangeUserInfoViewModel>();
            CreateMap<UserData, RegisterViewModel>();
            CreateMap<UserData, EditUserViewModel>();
        }
    }
}
