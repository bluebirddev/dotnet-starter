using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Bluebird.Core.Starter.Domain.Contracts.Services;
using Bluebird.Core.Starter.Domain.Models;

namespace Bluebird.Core.Starter.Controllers.V1
{
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        /// <summary>
        /// Create Movie
        /// </summary>
        [HttpPost("")]
        [Authorize]
        public async Task<ActionResult<Movie>> Create(MovieUpsertRequest movie)
        {
            var response = await _movieService.Create(movie);
            return Ok(response);
        }

        /// <summary>
        /// Get Movie
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> Get(Guid id)
        {
            var response = await _movieService.Get(id);
            return Ok(response);
        }

        /// <summary>
        /// List Movies
        /// </summary>
        [HttpGet("")]
        public async Task<ActionResult<ObjectList<Movie>>> List(int skip = 0, int take = 100, string name = null, string releaseYear = null)
        {
            var response = await _movieService.GetAll(skip, take, name, releaseYear);
            return Ok(response);
        }

    }
}
