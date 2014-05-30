
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BuildSeller.Service
{
    using global::Core.Interface;
    using global::Core.Model;

    public class CrudService<T> : ICrudService<T> where T : Entity, new()
    {
        public CrudService(IRepo<T> repo)
        {
            this.Repo = repo;
        }

     
        protected IRepo<T> Repo { get; set; }


        public IEnumerable<T> GetAll()
        {
            return this.Repo.GetAll();
        }

      
        public T Get(int id)
        {
            return this.Repo.Get(id);
        }

    
        public virtual int Create(T item)
        {
            this.Repo.Insert(item);
            return 1;
        }

      
        public void Save()
        {
            this.Repo.Save();
        }


        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {
            return this.Repo.GetAllIncluding(includeProperties);
        }

    
        public virtual void Delete(int id)
        {
            this.Repo.Delete(this.Repo.Get(id));
        }

     
        public void Update(T o)
        {
            this.Repo.Update(o);
        }

     
        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return this.Repo.Where(predicate);
        }
    }
}