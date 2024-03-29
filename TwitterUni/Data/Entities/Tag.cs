﻿using System.ComponentModel.DataAnnotations;
using TwitterUni.Data.BaseEntities;

namespace TwitterUni.Data.Entities
{
    public class Tag : BaseEntity
    {
        public Tag()
        {
            Tweets = new HashSet<Tweet>();
        }

        [Required]
        public string TagName { get; set; }
        public ICollection<Tweet> Tweets { get; set; }
    }
}
