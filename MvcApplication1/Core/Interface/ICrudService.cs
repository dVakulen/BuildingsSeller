
namespace Core.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Core.Model;

   
    public interface ICrudService<T>
        where T : Entity, new()
    {
      
        int Create(T item);

    
        void Delete(int id);

     
        T Get(int id);

        IEnumerable<T> GetAll();

        void Save();

     
        void Update(T obj);

        IEnumerable<T> Where(Expression<Func<T, bool>> func);

    
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
    }
}