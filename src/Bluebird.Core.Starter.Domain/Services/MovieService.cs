using Bluebird.Core.Starter.Domain.Contracts.Repositories;
using Bluebird.Core.Starter.Domain.Contracts.Services;
using Bluebird.Core.Starter.Domain.Models;
using Bluebird.Core.Starter.Domain.Services.Base;
using System;
using System.Threading.Tasks;

namespace Bluebird.Core.Starter.Domain.Services
{
    public class MovieService : BaseService<MovieUpsertRequest, MovieUpsertRequest, Movie, Guid>, IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        public MovieService(IMovieRepository movieRepository) : base(movieRepository)
        {
            _movieRepository = movieRepository;
        }

        public override async Task<Movie> Create(MovieUpsertRequest request)
        {
            var entity = new Movie
            {
                Genre = request.Genre,
                Director = request.Director,
                Name = request.Name,
                ReleaseYear = request.ReleaseYear
            };

            return await _movieRepository.Create(entity);
        }

        public async Task<ObjectList<Movie>> GetAll(int skip = 0, int take = 100, string name = null, string releaseYear = null)
        {
            return await _movieRepository.GetAll(skip, take, name, releaseYear);
        }

        public override async Task<Movie> Update(Guid id, MovieUpsertRequest request)
        {
            var entity = await Get(id);
            if (entity == null)
                throw new Exception("Movie does not exist");


            entity.Genre = request.Genre;
            entity.Director = request.Director;
            entity.Name = request.Name;
            entity.ReleaseYear = request.ReleaseYear;

            return await _movieRepository.Update(id, entity);
        }
    }
}
