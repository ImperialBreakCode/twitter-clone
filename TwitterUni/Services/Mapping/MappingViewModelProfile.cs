using AutoMapper;
using TwitterUni.Areas.Account.Models.Auth;
using TwitterUni.Data.Entities;
using TwitterUni.Services.ModelData;

namespace TwitterUni.Services.Mapping
{
    public class MappingViewModelProfile : Profile
    {
        public MappingViewModelProfile()
        {
            // to data
            CreateMap<RegisterViewModel, UserData>();

            // to viewModel
            CreateMap<UserData, RegisterViewModel>();
        }
    }
}
