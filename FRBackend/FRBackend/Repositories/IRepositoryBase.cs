using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FRBackend.Repositories
{
    public interface IRepositoryBase<T> where T : class
    {
        void Add(T entity);
        T Get(Expression<Func<T, bool>> where);
        T Get(int id);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        IQueryable<T> AsQueryable();
        IEnumerable<T> GetAll();
        void Commit(dynamic transaction);
        void Save();
        void Rollback(dynamic transaction);
        void Dispose();
        dynamic BeginTransaction();
        void EndTransaction(dynamic transaction);
    }
}
