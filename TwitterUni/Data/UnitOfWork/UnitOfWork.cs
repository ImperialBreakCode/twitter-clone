using TwitterUni.Data.Entities;
using TwitterUni.Data.Repositories;
using TwitterUni.Data.Repositories.Interfaces;

namespace TwitterUni.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TwitterDbContext _context;
        private IUserRepository _userRepository;
        private ITweetRepository _tweetRepository;
        private ITagRepository _tagRepository;
        private IRepository<Comment> _commentRepository;
        private IRepository<AppSettings> _appSettingsRepository;

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

        public ITagRepository TagRepository
        {
            get => _tagRepository = _tagRepository ?? new TagRepository(_context);
        }

        public IRepository<Comment> CommentRepository
        {
            get => _commentRepository = _commentRepository ?? new Repository<Comment>(_context);
        }

        public IRepository<AppSettings> AppSettingsRepository
        {
            get => _appSettingsRepository = _appSettingsRepository ?? new Repository<AppSettings>(_context);
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
