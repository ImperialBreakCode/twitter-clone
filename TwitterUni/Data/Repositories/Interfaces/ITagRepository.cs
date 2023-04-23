using TwitterUni.Data.Entities;

namespace TwitterUni.Data.Repositories.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        public Tag? GetTagByName(string name);
    }
}
