﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Infrastructure;

public class UnitOfWork<TContext> : IRepositoryFactory, IUnitOfWork<TContext>, IUnitOfWork where TContext : DbContext
{
    private readonly TContext _context;
    private bool disposed = false;
    private Dictionary<Type, object> repositories;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork{TContext}"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public UnitOfWork(TContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Gets the db context.
    /// </summary>
    /// <returns>The instance of type <typeparamref name="TContext"/>.</returns>
    public TContext DbContext => _context;

    /// <summary>
    /// Gets the specified repository for the <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <returns>An instance of type inherited from <see cref="IRepository{TEntity}"/> interface.</returns>
    public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class
    {
        if (repositories == null)
        {
            repositories = new Dictionary<Type, object>();
        }

        var type = typeof(TEntity);
        if (!repositories.ContainsKey(type))
        {
            repositories[type] = new Repository<TEntity>(_context);
        }

        return (IRepository<TEntity>)repositories[type];
    }

    /// <summary>
    /// Executes the specified raw SQL command.
    /// </summary>
    /// <param name="sql">The raw SQL.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>The number of state entities written to database.</returns>
    [Obsolete]
    public int ExecuteSqlCommand(string sql, params object[] parameters) => _context.Database.ExecuteSqlRaw(sql, parameters);

    /// <summary>
    /// Uses raw SQL queries to fetch the specified <typeparamref name="TEntity" /> data.
    /// </summary>
    /// <typeparam name="TEntity">The type of the entity.</typeparam>
    /// <param name="sql">The raw SQL.</param>
    /// <param name="parameters">The parameters.</param>
    /// <returns>An <see cref="IQueryable{T}" /> that contains elements that satisfy the condition specified by raw SQL.</returns>
    [Obsolete]
    public IQueryable<TEntity> FromSql<TEntity>(string sql, params object[] parameters) where TEntity : class => _context.Set<TEntity>().FromSqlRaw(sql, parameters);

    /// <summary>
    /// Saves all changes made in this context to the database.
    /// </summary>
    /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
    /// <returns>The number of state entries written to the database.</returns>
    public int SaveChanges(bool ensureAutoHistory = false)
    {
        if (ensureAutoHistory)
        {
            _context.EnsureAutoHistory();
        }

        return _context.SaveChanges();
    }

    /// <summary>
    /// Asynchronously saves all changes made in this unit of work to the database.
    /// </summary>
    /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
    public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false)
    {
        if (ensureAutoHistory)
        {
            _context.EnsureAutoHistory();
        }

        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Saves all changes made in this context to the database with distributed transaction.
    /// </summary>
    /// <param name="ensureAutoHistory"><c>True</c> if save changes ensure auto record the change history.</param>
    /// <param name="unitOfWorks">An optional <see cref="IUnitOfWork"/> array.</param>
    /// <returns>A <see cref="Task{TResult}"/> that represents the asynchronous save operation. The task result contains the number of state entities written to database.</returns>
    public async Task<int> SaveChangesAsync(bool ensureAutoHistory = false, params IUnitOfWork[] unitOfWorks)
    {
        // TransactionScope will be included in .NET Core v2.0
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                var count = 0;
                foreach (var unitOfWork in unitOfWorks)
                {
                    var uow = unitOfWork as UnitOfWork<DbContext>;
                    uow.DbContext.Database.UseTransaction(transaction.GetDbTransaction());
                    count += await uow.SaveChangesAsync(ensureAutoHistory);
                }

                count += await SaveChangesAsync(ensureAutoHistory);

                transaction.Commit();

                return count;
            }
            catch (Exception)
            {
                transaction.Rollback();

                throw;
            }
        }
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);

        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    /// <param name="disposing">The disposing.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                // clear repositories
                if (repositories != null)
                {
                    repositories.Clear();
                }

                // dispose the db context.
                _context.Dispose();
            }
        }

        disposed = true;
    }
}