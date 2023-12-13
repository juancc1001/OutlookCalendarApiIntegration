
namespace ProySuministros.DataAccess.Layer.Repository.Interfaces
{
    public interface _IBaseRepository <T> where T : class
    {
        T Get(object id);//return la query? cuando decidir traer o no las referencias?
        IEnumerable<T> GetAll ();
        void Insert (T entity);
        void Update (T entity);
        void Delete (object id);
        #region Async
        Task<T> GetAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        #endregion
    }
}
