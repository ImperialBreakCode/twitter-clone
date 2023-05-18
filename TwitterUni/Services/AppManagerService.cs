using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TwitterUni.Data.Entities;
using TwitterUni.Data.UnitOfWork;
using TwitterUni.Infrastructure.Constants;
using TwitterUni.Services.ApiFetching;
using TwitterUni.Services.ApiFetching.DTOs;
using TwitterUni.Services.Interfaces;

namespace TwitterUni.Services
{
	public class AppManagerService : IAppSettingsService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IFetchApi _fetchApi;
        private readonly Mapper _mapper;
		private readonly UserManager<User> _userManager;

        public AppManagerService(IUnitOfWork unitOfWork, 
			IFetchApi fetchApi, 
			Mapper mapper,
			UserManager<User> userManager)
		{
			_unitOfWork = unitOfWork;
			_fetchApi = fetchApi;
			_mapper = mapper;
			_userManager = userManager;
		}

		public void EnsureAppSettings()
		{
			bool appSettingsCheck = _unitOfWork.AppSettingsRepository.GetAll().Count() == 1;

			if (!appSettingsCheck)
			{
				AppSettings appSettings = new AppSettings() { DataIsLoaded = false };
				_unitOfWork.AppSettingsRepository.CreateOne(appSettings);
				_unitOfWork.Commit();
			}
		}

		public bool IsDataLoaded()
		{
			return _unitOfWork.AppSettingsRepository.GetAll().First().DataIsLoaded;
		}

		public async Task LoadDataFromApi()
		{
			var usersDto = await _fetchApi.FetchUserData(10);
			List<User> users = _mapper.Map<List<User>>(usersDto.DistinctBy(u => u.UserName));

			await LoadUsersAndTweets(users);
			await LoadComments();

			_unitOfWork.AppSettingsRepository.GetAll().First().DataIsLoaded = true;
			_unitOfWork.Commit();
		}

		private async Task LoadComments()
		{
			Random random = new Random();

			var commentDTOs = await _fetchApi.FetchUserPostData(390, 10);
			List<Comment> comments = _mapper.Map<List<Comment>>(commentDTOs);

			List<User> users = _unitOfWork.UserRepository.GetAll().ToList();
			List<Tweet> tweets = _unitOfWork.TweetRepository.GetAll()
				.Take(130).OrderBy(t => Guid.NewGuid()).ToList();

            int commentIndex = 0;
            foreach (Tweet tweet in tweets)
			{
                for (int i = 0; i < 3; i++)
				{
                    int randomUserIndex = random.Next(0, 10);
                    Comment comment = comments[commentIndex];

                    users[randomUserIndex].Comments.Add(comment);
                    tweet.Comments.Add(comment);

					commentIndex++;
                }
			}

			_unitOfWork.Commit();
		}

		private async Task LoadUsersAndTweets(List<User> users)
		{
			foreach (User user in users)
			{
				await _userManager.CreateAsync(user, "Pass_word123");
				await _userManager.AddToRoleAsync(user, RoleNames.User);

				var tweetDtos = await _fetchApi.FetchUserPostData(60, 250);
				LoadTweets(tweetDtos.ToList(), user);
			}

			_unitOfWork.Commit();
		}

		private void LoadTweets(List<UserPostDTO> tweets, User user)
		{
			foreach (var tweetDto in tweets)
			{
                Tweet tweet = _mapper.Map<Tweet>(tweetDto);
				user.Tweets.Add(tweet);

				if (tweetDto.TagNames != null)
				{
                    foreach (string tagname in tweetDto.TagNames)
                    {
						string name = tagname.Remove(0, 1);
						Tag? tag = _unitOfWork.TagRepository.GetTagByName(name);

						if (tag == null)
						{
							tag = new Tag() { TagName = name };
						}

                        tweet.Tags.Add(tag);
                    }
                }

				_unitOfWork.Commit();
            }
		}
	}
}
