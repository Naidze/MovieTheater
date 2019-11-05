using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheater;
using MovieTheater.Helpers;
using MovieTheater.Models;
using MovieTheater.Models.ViewModels;
using MovieTheater.Repositories;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = UserRoleDefaults.Admin)]
    public class MoviesController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly IMovieRepository _movieRepository;

        public MoviesController(IMovieRepository movieRepository)
        {
            _context = new MovieContext();
            _movieRepository = movieRepository;
        }

        // GET: api/Movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetMovies()
        {
            var movies = _movieRepository.GetAllMovies();
            return Ok(movies);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public ActionResult<Movie> GetMovie(int id)
        {
            if (!_movieRepository.MovieExists(id))
                return NotFound("Movie with id: '" + id + "' does not exist.");

            Movie movie = _movieRepository.GetMovie(id);

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }

        // GET: api/Movies/5
        [HttpGet("{id}/review")]
        public ActionResult<Review> GetMovieReview(int id)
        {
            if (!_movieRepository.MovieExists(id))
                return NotFound("Movie with id: '" + id + "' does not exist.");

            Movie movie = _movieRepository.GetMovie(id);

            if (movie.Review == null)
            {
                return NotFound();
            }

            return Ok(movie.Review);
        }

        // PUT: api/Movies/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Movies
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(CreateMovieViewModel model)
        {
            Category category = _context.Categories.Where(cat => cat.Id == model.CategoryID).FirstOrDefault();
            Movie movie = new Movie
            {
                Author = model.Author,
                Title = model.Title,
                Description = model.Description,
                Year = model.Year,
                ImageURL = model.ImageURL,
                Category = category
            };
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movies/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
