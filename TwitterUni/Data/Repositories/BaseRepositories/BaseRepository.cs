using Microsoft.EntityFrameworkCore;
using TwitterUni.Data.BaseEntities;

namespace TwitterUni.Data.Repositories.BaseRepositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class, IEntityId
    {
        private readonly TwitterDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public BaseRepository(TwitterDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        protected TwitterDbContext Context { get => _context; }
        protected DbSet<TEntity> DbSetData { get => _dbSet; }

        public void CreateOne(TEntity entity)
        {
            DbSetData.Add(entity);
        }

        public bool DeleteOne(string id)
        {

            var entity = _dbSet.Find(id);

            if (entity is not null)
            {
                DbSetData.Remove(entity);
                return true;
            }

            return false;
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSetData;
        }

        public TEntity? GetOne(string id)
        {
            return DbSetData.Find(id);
        }

        public void UpdateOne(TEntity entity)
        {
            DbSetData.Update(entity);
        }
        public void Save()
        {
            Context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
