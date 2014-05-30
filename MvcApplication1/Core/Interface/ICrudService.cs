// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICrudService.cs" company="nixsolutions">
//   (c) by nix
// </copyright>
// <summary>
//   The CrudService interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Core.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Core.Model;

    /// <summary>
    /// The CrudService interface.
    /// </summary>
    /// <typeparam name="T">
    /// </typeparam>
    public interface ICrudService<T>
        where T : Entity, new()
    {
        /// <summary>
        /// The create.
        /// </summary>
        /// <param name="item">
        /// The item.
        /// </param>
        /// <returns>
        /// The <see cref="int"/>.
        /// </returns>
        int Create(T item);

        /// <summary>
        /// The delete.
        /// </summary>
        /// <param name="id">
        /// The id.
        /// </param>
        void Delete(int id);

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
        ///     The <see cref="IEnumerable" />.
        /// </returns>
        IEnumerable<T> GetAll();

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
        /// The where.
        /// </summary>
        /// <param name="func">
        /// The func.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        IEnumerable<T> Where(Expression<Func<T, bool>> func);

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
    }
}