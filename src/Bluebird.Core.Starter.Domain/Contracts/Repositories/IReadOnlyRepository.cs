using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Bluebird.Core.Starter.Domain.Contracts.Repositories
{
    public interface IReadOnlyRepository<T, S>
    {
        Task<T> Get(S id);
    }
}
