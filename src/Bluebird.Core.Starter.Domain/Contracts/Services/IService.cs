using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bluebird.Core.Starter.Domain.Contracts.Services
{
    public interface IService<TCreateDTO, TUpdateDTO, TEntity, TKey> : IReadOnlyService<TEntity, TKey>
    {
        Task<TEntity> Create(TCreateDTO entity);
        Task<TEntity> Update(TKey id, TUpdateDTO entity);
        Task Delete(TKey id);
    }
}
