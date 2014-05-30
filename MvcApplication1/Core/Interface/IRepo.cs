// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepo.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The Repo interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Core.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// The Repo interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface IRepo<T>
    {
        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        void Delete(T obj);

        /// <summary>
        /// The get all including.
        /// </summary>
        /// <param name="includeProperties">
        /// The include properties.
        /// </param>
        /// <returns>
        /// The <see cref="IQueryable"/>.
        /// </returns>
        IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// The get.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <returns>
        /// The <see cref="T"/>.
        /// </returns>
        T Get(int id);

        /// <summary>
        ///     The get all.
        /// </summary>
        /// <returns>
        ///     The <see cref="IList" />.
        /// </returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        void Insert(T obj);

        /// <summary>
        ///     The save.
        /// </summary>
        void Save();

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="obj">
        /// The obj.
        /// </param>
        void Update(T obj);

        /// <summary>
        /// The update.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        /// <param name="obj">
        /// The obj.
        /// </param>
        void Update(int id, T obj);

        /// <summary>
        /// The where.
        /// </summary>
        /// <param name="predicate">
        /// The predicate.
        /// </param>
        /// <returns>
        /// The <see cref="IList"/>.
        /// </returns>
        IList<T> Where(Expression<Func<T, bool>> predicate);
    }
}