using TwitterUni.Infrastructure.Constants;
using TwitterUni.Services.Interfaces;

namespace TwitterUni.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void SaveTweetImage(Image image, string fileName, string userName)
        {
            string path = Path.Combine(_environment.WebRootPath, StaticFilePaths.TweetImagesPath, userName);
            Directory.CreateDirectory(path);
            SaveImage(image, path, fileName);
        }

        public void SaveBackgroundImage(Image image, string fileName)
        {
            string bgPath = Path.Combine(_environment.WebRootPath, StaticFilePaths.BackgroundImages);
            SaveImage(image, bgPath, fileName);
        }

        public void SaveProfileImage(Image image, string fileName)
        {
            string profilePath = Path.Combine(_environment.WebRootPath, StaticFilePaths.ProfileImages);
            SaveImage(image, profilePath, fileName, 480, 480);
        }

        public void SaveImage(Image image, string path, string fileName, int maxHeight = 1080, int maxWidth = 1920)
        {
            double aspectRatioW = (double)image.Width / image.Height;
            double aspectRatioH = (double)image.Height / image.Width;

            if (image.Width > maxWidth)
            {
                image.Mutate(x => x.Resize(maxWidth, (int)(maxWidth * aspectRatioH)));
            }

            if (image.Height > maxHeight)
            {
                image.Mutate(x => x.Resize((int)(maxHeight * aspectRatioW), maxHeight));
            }

            image.SaveAsJpeg(path + $"\\{fileName}");
        }

        public void DeleteTweetImage(string filePath)
        {
            string path = Path.Combine(_environment.WebRootPath, StaticFilePaths.TweetImagesPath, filePath);
            File.Delete(path);
        }
    }
}
