using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Bluebird.Core.Starter.Domain.Contracts.Integrations;
using Bluebird.Core.Starter.Domain.Contracts.Services;
using Bluebird.Core.Starter.Domain.Models;

namespace Bluebird.Core.Starter.Controllers.V1
{
    //[Authorize]
    [ApiController]
    [Produces("application/json")]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly ILogger<MovieController> _logger;
        private readonly IMovieService _movieService;
        private readonly UserManager<IdentityUser> _userManager;

        public MovieController(ILogger<MovieController> logger, IMovieService movieService, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            _movieService = movieService;
            _userManager = userManager;
        }

        /// <summary>
        /// Create Movie
        /// </summary>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<Movie>> Create(MovieUpsertRequest movie)
        {
            var response = await _movieService.Create(movie);
            return Ok(response);
        }

        /// <summary>
        /// Get Movie
        /// </summary>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<Movie>> Get(Guid id)
        {
            var response = await _movieService.Get(id);
            return Ok(response);
        }

        /// <summary>
        /// List Movies
        /// </summary>
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<ObjectList<Movie>>> List(int skip = 0, int take = 100, string name = null, string releaseYear = null)
        {
            var response = await _movieService.GetAll(skip, take, name, releaseYear);
            return Ok(response);
        }

    }
}
