namespace TwitterUni.Services.Interfaces
{
	public interface IAppSettingsService
	{
		void EnsureAppSettings();
		Task LoadDataFromApi();
		bool IsDataLoaded();
	}
}
