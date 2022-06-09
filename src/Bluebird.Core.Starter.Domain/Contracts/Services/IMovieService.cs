using Bluebird.Core.Starter.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bluebird.Core.Starter.Domain.Contracts.Services
{
    public interface IMovieService: IService<MovieUpsertRequest, MovieUpsertRequest, Movie, Guid>
    {
        Task<ObjectList<Movie>> GetAll(int skip = 0, int take = 100, string name = null, string releaseYear = null);
    }
}
