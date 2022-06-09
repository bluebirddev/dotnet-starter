using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Bluebird.Core.Starter.Domain.Contracts.Repositories;
using Bluebird.Core.Starter.Domain.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bluebird.Core.Starter.Repository.PostgresSql.Repositories.Base
{
    public abstract class BaseRepository<T, S>: IRepository<T, S> where T : class
    {
        private readonly DotnetStarterContext _dbContext;
        private readonly ILogger<BaseRepository<T, S>> _logger;

        public BaseRepository(ILogger<BaseRepository<T, S>> logger, DotnetStarterContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<T> Create(T entity)
        {
            var entityAdded = await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return (T)entityAdded.Entity;
        }

        public async Task Delete(S id)
        {
            var entity = await Get(id);
            _dbContext.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task<T> Get(S id)
        {
            return (T)await _dbContext.FindAsync(typeof(T), id);
        }

        public async Task<T> Update(S id, T entity)
        {
            var serverEntity = await Get(id);
            Utils.Copy(entity, serverEntity, new string[] { "Id" });
            await _dbContext.SaveChangesAsync();
            var updatedEntity = await Get(id);
            return updatedEntity;
        }

        public async Task Update()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<long> Count(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> set = _dbContext.Set<T>().AsQueryable<T>();
            if (filter != null)
                set = set.Where(filter);
            return await set.LongCountAsync();
        }

        public async Task<ObjectList<T>> List(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, int skip = 0, int? take = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable<T>();
            if (include != null)
                query = include(query);

            if (filter != null)
                query = query.Where(filter);

            if (orderBy != null)
                query = orderBy(query);

            query = query.Skip(skip);
            if (take != null)
                query = query.Take(take.Value);

            var count = await Count(filter);
            return new ObjectList<T>
            {
                Items = query.ToList(), // remove tracking
                TotalCount = count,
                RawQuery = query.ToQueryString()
            };
        }
    }
}
