using System.Threading.Tasks;

namespace Bluebird.Core.Starter.Domain.Contracts.Services
{
    public interface IReadOnlyService<T, S>
    {
        Task<T> Get(S id);
    }
}
