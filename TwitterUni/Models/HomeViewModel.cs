﻿using TwitterUni.Services.ModelData;

namespace TwitterUni.Models
{
    public class HomeViewModel
    {
        public ICollection<UserData> Users { get; set; }
        public ICollection<TweetData> Tweets { get; set; }
        public ICollection<TagData> Tags { get; set; }
    }
}
