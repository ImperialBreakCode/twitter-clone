﻿using Microsoft.EntityFrameworkCore;
using TwitterUni.Data.BaseEntities;
using TwitterUni.Data.Repositories.Interfaces;

namespace TwitterUni.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntityId
    {
        private readonly TwitterDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(TwitterDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        protected TwitterDbContext Context { get => _context; }
        protected DbSet<TEntity> DbSetData { get => _dbSet; }

        public async Task CreateMany(params TEntity[] entities)
        {
            await DbSetData.AddRangeAsync(entities);
        }

        public void CreateOne(TEntity entity)
        {
            DbSetData.Add(entity);
        }

        public void DeleteOne(string id)
        {
            var entity = _dbSet.Find(id);

            if (entity is not null)
            {
                DbSetData.Remove(entity);
            }
        }

        public void DeleteRange(params TEntity[] entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbSetData;
        }

        public virtual TEntity? GetOne(string id)
        {
            return DbSetData.Find(id);
        }

        public void UpdateOne(TEntity entity)
        {
            DbSetData.Update(entity);
        }
    }
}
