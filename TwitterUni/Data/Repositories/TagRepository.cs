﻿using Microsoft.EntityFrameworkCore;
using TwitterUni.Data.Entities;
using TwitterUni.Data.Repositories.Interfaces;

namespace TwitterUni.Data.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(TwitterDbContext context) : base(context)
        {
        }

        public Tag? GetTagByName(string name)
        {
            return Context.Tags
                .Include(t => t.Tweets).ThenInclude(twt => twt.Author)
                .Include(t => t.Tweets).ThenInclude(twt => twt.Comments)
                .Include(t => t.Tweets).ThenInclude(twt => twt.UserLikes)
                .Include(t => t.Tweets).ThenInclude(twt => twt.Retweets).ThenInclude(r => r.RetweetedBy)
                .FirstOrDefault(t => t.TagName == name);
        }
    }
}
