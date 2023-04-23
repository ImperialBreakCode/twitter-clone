using TwitterUni.Data.Repositories.Interfaces;

namespace TwitterUni.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ITweetRepository TweetRepository { get; }
        ITagRepository TagRepository { get; }

        void Commit();
        Task CommitAsync();
    }
}
