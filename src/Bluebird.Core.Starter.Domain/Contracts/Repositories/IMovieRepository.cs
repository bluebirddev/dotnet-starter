using Bluebird.Core.Starter.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Bluebird.Core.Starter.Domain.Contracts.Repositories
{
    public interface IMovieRepository: IRepository<Movie, Guid>
    {
        Task<ObjectList<Movie>> GetAll(int skip = 0, int take = 100, string name = null, string releaseYear = null);
    }
}
