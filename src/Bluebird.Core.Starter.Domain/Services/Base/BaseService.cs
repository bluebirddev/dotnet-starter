using Bluebird.Core.Starter.Domain.Contracts.Repositories;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bluebird.Core.Starter.Domain.Services.Base
{
    public abstract class BaseService<TCreateDTO, TUpdateDTO, TEntity, TKey> : BaseReadOnlyService<TEntity, TKey>
    {
        private readonly IRepository<TEntity, TKey> _baseRepository;
        protected BaseService(IRepository<TEntity, TKey> countableRepository): base(countableRepository)
        {
            _baseRepository = countableRepository;
        }

        public abstract Task<TEntity> Create(TCreateDTO entity);

        public virtual async Task Delete(TKey id)
        {
            await _baseRepository.Delete(id);
        }

        public abstract Task<TEntity> Update(TKey id, TUpdateDTO entity);
    }
}
