using TwitterUni.Data.Repositories;
using TwitterUni.Data.Repositories.BaseRepositories;

namespace TwitterUni.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TwitterDbContext _context;
        private IUserRepository _userRepository;
        private ITweetRepository _tweetRepository;

        public UnitOfWork(TwitterDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository is null)
                {
                    _userRepository = new UserRepository(_context);
                }

                return _userRepository;
            }
        }

        public ITweetRepository TweetRepository
        {
            get => _tweetRepository = _tweetRepository ?? new TweetRepository(_context);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
