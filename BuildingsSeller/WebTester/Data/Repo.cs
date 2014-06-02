
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using BuildSeller.Core.Model;
using BuildSeller.Core.Repository;

namespace BuildSeller.Data
{

    public class Repo<T> : IRepo<T>
    where T : Entity, new()
    {

        protected readonly DbContext dbContext;

        public Repo(IDbContextFactory dbContextFactory)
        {
            this.dbContext = dbContextFactory.GetContext();
        }

        public virtual void Delete(T obj)
        {
            this.dbContext.Set<T>().Remove(obj);
            this.dbContext.SaveChanges();
        }

        public T Get(int id)
        {
            return this.dbContext.Set<T>().Find(id);
        }

        public virtual IQueryable<T> GetAll()
        {
            return this.dbContext.Set<T>();
        }

        public void Insert(T obj)
        {
            this.dbContext.Set<T>().Add(obj);
            this.dbContext.SaveChanges();
        }

        public void Save()
        {
            this.dbContext.SaveChanges();
        }

        public void Update(T obj)
        {
            this.dbContext.Set<T>().AddOrUpdate(obj);
            this.dbContext.SaveChanges();
        }

        public void Update(int id, T obj)
        {
            this.dbContext.Set<T>().AddOrUpdate(obj);
            this.dbContext.SaveChanges();
        }

        public virtual IList<T> Where(Expression<Func<T, bool>> predicate)
        {

            return this.dbContext.Set<T>().Where(predicate).ToList();
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> queryable = this.GetAll();
            foreach (var includeProperty in includeProperties)
            {
                queryable = queryable.Include(includeProperty);
            }

            return queryable;
        }
    }
}
