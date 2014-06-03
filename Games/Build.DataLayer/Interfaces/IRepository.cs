#region Using statements



#endregion

namespace Build.DataLayer.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository<T>
    {
        void Insert(T entity);

        void InsertAll(IEnumerable<T> entities);

        void Delete(T entity);

        void DeleteAll(IEnumerable<T> entities);

        IQueryable<T> GetAll();

        T Get(Expression<Func<T, bool>> predicate);

        IQueryable<T> Where(Expression<Func<T, bool>> predicate);

        void SubmitChanges();

        T FirstOrDefault();
    }
}