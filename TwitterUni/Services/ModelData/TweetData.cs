namespace TwitterUni.Services.ModelData
{
    public class TweetData
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TextContent { get; set; }
        public string Image { get; set; }
        public UserData Author { get; set; }
    }
}
