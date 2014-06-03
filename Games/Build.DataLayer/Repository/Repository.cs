#region Using statements



#endregion

namespace Build.DataLayer.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.Linq;
    using System.Linq.Expressions;

    using Build.DataLayer.Context;
    using Build.DataLayer.Interfaces;

    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly BuildContext context;
        private Table<TEntity> dataTable;

        #region Constructor
        public Repository(BuildContext dataContext)
        {
            this.context = dataContext;
            this.dataTable = this.context.GetTable<TEntity>();
        } 
        #endregion

        public void Insert(TEntity entity)
        {
            this.dataTable.InsertOnSubmit(entity);
        }

        public void InsertAll(IEnumerable<TEntity> entities)
        {
            this.dataTable.InsertAllOnSubmit(entities);
        }

        public void Delete(TEntity entity)
        {
            this.dataTable.DeleteOnSubmit(entity);
        }

        public void DeleteAll(IEnumerable<TEntity> entities)
        {
            this.dataTable.DeleteAllOnSubmit(entities);
        }
        
        public IQueryable<TEntity> GetAll()
        {
            return this.dataTable;
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dataTable.FirstOrDefault(predicate);
        }

        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return this.dataTable.Where(predicate);
        }

        public TEntity FirstOrDefault()
        {
            return this.dataTable.FirstOrDefault();
        }

        public void SubmitChanges()
        {
            this.context.SubmitChanges();
        }
    }
}