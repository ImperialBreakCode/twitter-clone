using TwitterUni.Data.UnitOfWork;
using TwitterUni.Services.ApiFetching;
using TwitterUni.Services.Interfaces;
using TwitterUni.Services;
using AutoMapper;
using TwitterUni.Services.Mapping;

namespace TwitterUni.Infrastructure.Extensions
{
    public static class AppServiceInjection
    {
        public static void InjectAppServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IImageService, ImageService>();
            services.AddTransient<ITweetService, TweetService>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<ICommentService, CommentService>();
            services.AddTransient<IAppSettingsService, AppManagerService>();
            services.AddScoped<IFetchApi, FetchApi>();
        }

        public static void AddMapping(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingServiceDataProfile>();
                cfg.AddProfile<MappingViewModelProfile>();
                cfg.AddProfile<MappingDTOsProfile>();
            });

            services.AddSingleton(new Mapper(config));
        }
    }
}
