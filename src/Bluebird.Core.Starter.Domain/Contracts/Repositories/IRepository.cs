using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bluebird.Core.Starter.Domain.Contracts.Repositories
{
    public interface IRepository<T, S> : IReadOnlyRepository<T, S>
    {
        Task<T> Create(T entity);
        Task<T> Update(S id, T entity);
        Task Delete(S id);
    }
}
