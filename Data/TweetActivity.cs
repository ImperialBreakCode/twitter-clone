using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class TweetActivity
    {
        public int TweetActivityId { get; set; }
        public string Type { get; set; }
        public Tweet Tweet { get; set; }
        public User Doer { get; set; }
    }
}
