using ProySuministros.DataAccess.Layer.Models;
using ProySuministros.DataAccess.Layer.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ProySuministros.DataAccess.Layer.Repositories.Implementatios.DataBase
{
    public abstract class _BaseRepository<T> : _IBaseRepository<T> where T : _Entity
    {
        protected readonly DbContext _context;
        private readonly DbSet<T> _dbSet;
        public _BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual void Insert(T entity)
        {
            _dbSet.Add(entity);
        }
        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
        }
        public virtual T Get(object id)
        {
            return _dbSet.Where(t => t.Id.Equals(id)).Single();
        }
        public virtual void Delete(object id)
        {
            _dbSet.Remove(Get(id));
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public virtual Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }


        public virtual Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task InsertAsync(T entity)
        {
            throw new NotImplementedException();
        }


        public virtual Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
