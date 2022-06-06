using DotNetCoreScaffoldingSqlServer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FRBackend.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        public FinancialReportDBContext db;
        private DbSet<T> _dbSet;

        //private readonly IDbContextTransaction transaction;

        internal RepositoryBase()
        {
            db = new FinancialReportDBContext(new DbContextOptions<FinancialReportDBContext>());
            _dbSet = db.Set<T>();
        }
        public virtual void Add(T entity)
        {
            _dbSet.Add(entity);
            db.SaveChanges();
        }
        public virtual T Get(Expression<Func<T, bool>> where)
        {
            DetachAllEntities();
            return _dbSet.Where(where).FirstOrDefault();
        }
        public virtual T Get(int id)
        {
            DetachAllEntities();
            return _dbSet.Find(id);
        }
        public virtual void Update(T entity)
        {
            _dbSet.Update(entity);
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
        }
        public virtual void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            var objects = _dbSet.Where(where).AsEnumerable();
            foreach (var obj in objects)
            {
                _dbSet.Remove(obj);
            }
            db.SaveChanges();
        }
        public virtual IQueryable<T> AsQueryable() => _dbSet.AsNoTracking().AsQueryable();
        public virtual IEnumerable<T> GetAll() => _dbSet.ToList();
        public virtual void Commit(dynamic transaction)
        {
            //transaction.Commit();
            //DetachAllEntities();
            transaction.Commit();
            transaction.Dispose();
        }
        public virtual void Save() => db.SaveChanges();
        public virtual void Rollback(dynamic transaction)
        {
            //transaction.Rollback();
            //DetachAllEntities();
            transaction.Rollback();
            transaction.Dispose();
        }
        public virtual void Dispose()
        {
            db.Dispose();
            //transaction.Dispose();
        }
        public void DetachAllEntities()
        {
            IEnumerable<EntityEntry> changedEntriesCopy = db.ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified
                        || x.State == EntityState.Added
                        || x.State == EntityState.Deleted);
            foreach (var entity in changedEntriesCopy)
            {
                entity.State = EntityState.Detached;
            }
        }
        public dynamic BeginTransaction()
        {
            var das = db.Database.BeginTransaction();
            das.Dispose();
            return db.Database.BeginTransaction();
        }
        public virtual void EndTransaction(dynamic transaction)
        {
            transaction.Dispose();
        }
    }
}
