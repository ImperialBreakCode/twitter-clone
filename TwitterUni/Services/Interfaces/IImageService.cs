namespace TwitterUni.Services.Interfaces
{
    public interface IImageService
    {
        void SaveImage(Image image, string path, string fileName, int maxHeight, int maxWidth);
        void SaveProfileImage(Image image, string fileName);
        void SaveBackgroundImage(Image image, string fileName);
        void SaveTweetImage(Image image, string fileName, string userName);
        void DeleteTweetImage(string tweetImagePath);
        void DeleteAllUserTweetImages(string userName);
        void DeleteUserImage(string filePath);
    }
}
