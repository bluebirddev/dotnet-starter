using Bluebird.Core.Starter.Domain.Contracts.Repositories;
using Bluebird.Core.Starter.Domain.Contracts.Services;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bluebird.Core.Starter.Domain.Services.Base
{
    public abstract class BaseReadOnlyService<TEntity, TKey> : IReadOnlyService<TEntity, TKey>
    {
        private readonly IReadOnlyRepository<TEntity, TKey> _baseRepository;
        protected BaseReadOnlyService(IReadOnlyRepository<TEntity, TKey> countableRepository)
        {
            _baseRepository = countableRepository;
        }

        public async Task<TEntity> Get(TKey id)
        {
            return await _baseRepository.Get(id);
        }
    }
}
