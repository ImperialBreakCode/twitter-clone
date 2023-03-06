using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Comments
    {
        public int CommentsId { get; set; }
        public string TextContent { get; set; }
        public DateTime PublishDate { get; set; }
        public Tweet ParentTweet { get; set; }
        public User Author { get; set; }
        public ICollection<User> Likes { get; set; }
    }
}
