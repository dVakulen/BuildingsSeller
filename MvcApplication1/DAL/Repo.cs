// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Repo.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The repo.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Linq.Expressions;

    using Core.Interface;
    using Core.Model;

    /// <summary>
    /// The repo.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class Repo<T> : IRepo<T>
        where T : Entity, new()
    {
        /// <summary>
        /// The db context.
        /// </summary>
        protected readonly DbContext dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="Repo{T}"/> class.
        /// </summary>
        /// <param name="dbContextFactory">
        /// The db context factory.
        /// </param>
        public Repo(IDbContextFactory dbContextFactory)
        {
            this.dbContext = dbContextFactory.GetContext();
        }


        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        public virtual void Delete(T obj)
        {
            this.dbContext.Set<T>().Remove(obj);
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        public T Get(int id)
        {
            return this.dbContext.Set<T>().Find(id);
        }

        /// <summary>
        ///     The get all.
        /// </summary>
        /// <returns>
        ///     The <see cref="IList" />.
        /// </returns>
        public virtual IQueryable<T> GetAll()
        {
            return this.dbContext.Set<T>();
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        public void Insert(T obj)
        {
            this.dbContext.Set<T>().Add(obj);
            this.dbContext.SaveChanges();
        }

        /// <summary>
        ///     The save.
        /// </summary>
        public void Save()
        {
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        public void Update(T obj)
        {
            this.dbContext.Set<T>().AddOrUpdate(obj);
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="obj">
        /// The obj.
        /// </param>
        public void Update(int id, T obj)
        {
            this.dbContext.Set<T>().AddOrUpdate(obj);
            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// The where.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        public virtual IList<T> Where(Expression<Func<T, bool>> predicate)
        {
           
            return this.dbContext.Set<T>().Where(predicate).ToList();
        }

        /// <summary>
        /// The get all including.
        /// </summary>
        /// <param name="includeProperties">
        /// The include properties.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
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