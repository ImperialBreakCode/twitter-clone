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
            return Context.Tags.FirstOrDefault(t => t.TagName == name);
        }
    }
}
