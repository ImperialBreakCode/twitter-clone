using TwitterUni.Data.Repositories.BaseRepositories;

namespace TwitterUni.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ITweetRepository TweetRepository { get; }

        void Commit();
        Task CommitAsync();
    }
}
