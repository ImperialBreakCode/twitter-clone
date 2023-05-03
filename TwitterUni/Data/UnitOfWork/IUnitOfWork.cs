using TwitterUni.Data.Entities;
using TwitterUni.Data.Repositories.Interfaces;

namespace TwitterUni.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }
        ITweetRepository TweetRepository { get; }
        ITagRepository TagRepository { get; }
        IRepository<Comment> CommentRepository { get; }
        IRepository<AppSettings> AppSettingsRepository { get; }

        void Commit();
        Task CommitAsync();
    }
}
