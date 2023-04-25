namespace TwitterUni.Infrastructure.Constants
{
    public static class StaticFilePaths
    {
        public const string ImagesPath = "content\\images";
        public const string ProfileImSubfolder = "profileImages";
        public const string BgImSubfolder = "backgroundImages";
        public const string ProfileImages = ImagesPath + $"\\{ProfileImSubfolder}";
        public const string BackgroundImages = ImagesPath + $"\\{BgImSubfolder}";
        public const string TweetImagesPath = ImagesPath + "\\tweets";
    }
}
