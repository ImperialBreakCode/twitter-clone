namespace TwitterUni.Data.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity? GetOne(string id);
        void UpdateOne(TEntity entity);
        void DeleteOne(string id);
        void CreateOne(TEntity entity);
        Task CreateMany(params TEntity[] entities);
        void DeleteRange(params TEntity[] entities);
    }
}
