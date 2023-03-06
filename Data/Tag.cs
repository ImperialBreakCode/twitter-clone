﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Tag
    {
        public int TagId { get; set; }
        public string TagName { get; set; }
        public ICollection<Tweet> Tweets { get; set; }
    }
}
