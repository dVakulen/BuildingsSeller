// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CrudService.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The crud service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BuildSeller.Service
{
    using global::Core.Interface;
    using global::Core.Model;

    /// <summary>
    /// The crud service.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public class CrudService<T> : ICrudService<T> where T : Entity, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CrudService{T}"/> class.
        /// </summary>
        /// <param name="repo">
        /// The repo.
        /// </param>
        public CrudService(IRepo<T> repo)
        {
            this.Repo = repo;
        }

        /// <summary>
        ///     The repo.
        /// </summary>
        protected IRepo<T> Repo { get; set; }

        /// <summary>
        ///     The get all.
        /// </summary>
        /// <returns>
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        public IEnumerable<T> GetAll()
        {
            return this.Repo.GetAll();
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
            return this.Repo.Get(id);
        }

        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        public virtual int Create(T item)
        {
            this.Repo.Insert(item);
            return 1;
        }

        /// <summary>
        ///     The save.
        /// </summary>
        public void Save()
        {
            this.Repo.Save();
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
            return this.Repo.GetAllIncluding(includeProperties);
        }

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        public virtual void Delete(int id)
        {
            this.Repo.Delete(this.Repo.Get(id));
        }

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="o">
        /// The o.
        /// </param>
        public void Update(T o)
        {
            this.Repo.Update(o);
        }

        /// <summary>
        /// The where.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        public IEnumerable<T> Where(Expression<Func<T, bool>> predicate)
        {
            return this.Repo.Where(predicate);
        }
    }
}