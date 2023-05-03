using TwitterUni.Data.Entities;
using TwitterUni.Data.UnitOfWork;
using TwitterUni.Services.Interfaces;

namespace TwitterUni.Services
{
	public class AppSettingsService : IAppSettingsService
	{
		private readonly IUnitOfWork _unitOfWork;

		public AppSettingsService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
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

		public void LoadDataFromApi()
		{
			throw new NotImplementedException();
		}
	}
}
