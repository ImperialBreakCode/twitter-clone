namespace Data
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePic { get; set; }
        public string BackgroundPhoto { get; set; }
        public string Bio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Tweet> Tweets { get; set; }
        public List<Tweet> LikedTweets { get; set; }
        public List<User> Followers { get; set; }
        public List<User> Following { get; set; }
    }
}