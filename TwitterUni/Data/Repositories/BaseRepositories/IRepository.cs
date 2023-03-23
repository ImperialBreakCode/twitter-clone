namespace TwitterUni.Data.Repositories.BaseRepositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetAll();
        TEntity? GetOne(string id);
        void UpdateOne(TEntity entity);
        bool DeleteOne(string id);
        void CreateOne(TEntity entity);
        void Save();
        Task SaveAsync();
    }
}
