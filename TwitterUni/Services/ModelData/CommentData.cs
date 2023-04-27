using TwitterUni.Data.Entities;

namespace TwitterUni.Services.ModelData
{
    public class CommentData
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string TextContent { get; set; }
        public UserData Author { get; set; }
        public ICollection<UserData> Likes { get; set; }
    }
}
