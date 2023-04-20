using AutoMapper;
using TwitterUni.Areas.Account.Models.Auth;
using TwitterUni.Models.User;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Services.Mapping
{
    public class MappingViewModelProfile : Profile
    {
        public MappingViewModelProfile()
        {
            // to data
            CreateMap<RegisterViewModel, UserData>();
            CreateMap<EditUserViewModel, UserData>();

            // to viewModel
            CreateMap<UserData, RegisterViewModel>();
            CreateMap<UserData, EditUserViewModel>();
        }
    }
}
