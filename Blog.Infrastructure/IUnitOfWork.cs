﻿using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Infrastructure;

public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Gets the specified repository for the <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
    IRepository<TEntity> GetRepository<TEntity>() where TEntity : class;

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="ensureAutoHistory"><c>True</c> if sayve changes ensure auto record the change history.</param>
    /// <returns>The number of state entries written to the database.</returns>
    int SaveChanges(bool ensureAutoHistory = false);

    /// <summary>
    /// Asynchronously saves all changes made in this unit of work to the database.
    /// </summary>
    /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
    Task<int> SaveChangesAsync(bool ensureAutoHistory = false);

    /// <summary>
    /// Executes the specified raw SQL command.
    /// </summary>
    /// <param name="sql">The raw SQL.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The number of state entities written to database.</returns>
    int ExecuteSqlCommand(string sql, params object[] parameters);

    /// <summary>
    /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity"/> data.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="sql">The raw SQL.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>An <see cref="IQueryable{T}"/> that contains elements that satisfy the condition specified by raw SQL.</returns>
    IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class;
}