namespace TwitterUni.Models.Comment
{
    public class CreateCommentModel
    {
        public string Id { get; set; }
        public string TweetId { get; set; }
        public string Text { get; set; }
    }
}
