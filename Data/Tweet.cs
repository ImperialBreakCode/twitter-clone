using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Tweet
    {
        public int TweetId { get; set; }
        public string Caption { get; set; }
        public string Image { get; set; }
        public int Likes { get; set; }
        public int Retweets { get; set; }
        public int Views { get; set; }
        public DateTime PublishDate { get; set; }
        public User User { get; set; }
        public List<Tweet> Replies { get; set; }
        public Tweet ParentTweet { get; set; }
    }
}
