namespace TwitterUni.Services.Interfaces
{
    public interface IImageService
    {
        public void SaveImage(Image image, string path, string fileName, int maxHeight, int maxWidth);
        public void SaveProfileImage(Image image, string fileName);
        public void SaveBackgroundImage(Image image, string fileName);
    }
}
