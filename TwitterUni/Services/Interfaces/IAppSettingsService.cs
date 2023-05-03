namespace TwitterUni.Services.Interfaces
{
	public interface IAppSettingsService
	{
		void EnsureAppSettings();
		void LoadDataFromApi();
		bool IsDataLoaded();
	}
}
