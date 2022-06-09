using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Bluebird.Core.Starter.Domain.Contracts.Repositories;
using Bluebird.Core.Starter.Domain.Models;
using Bluebird.Core.Starter.Repository.PostgresSql.Repositories.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bluebird.Core.Starter.Repository.PostgresSql.Repositories
{
    public class MovieRepository : BaseRepository<Movie, Guid>, IMovieRepository
    {

        public MovieRepository(ILogger<BaseRepository<Movie, Guid>> logger, DotnetStarterContext dbContext) : base(logger, dbContext)
        {
        }

        public override async Task<Movie> Get(Guid id)
        {
            var result = await base.List((movie) => movie.Id == id, null, 0, 1);
            return result.Items.FirstOrDefault();
        }

        public async Task<ObjectList<Movie>> GetAll(int skip = 0, int take = 100, string name = null, string releaseYear = null)
        {
            var result = await base.List((movie) => 
            (name == null || movie.Name == name) &&
            (releaseYear == null || movie.ReleaseYear == releaseYear),
            null, skip, take);
            return result;
        }
    }
}
