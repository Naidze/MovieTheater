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
        private readonly ICategoryRepository _categoryRepository;
        private readonly IQuoteRepository _quoteRepository;

        public MoviesController(IMovieRepository movieRepository, ICategoryRepository categoryRepository, IQuoteRepository quoteRepository)
        {
            _context = new MovieContext();
            _movieRepository = movieRepository;
            _categoryRepository = categoryRepository;
            _quoteRepository = quoteRepository;
        }

        // GET: api/Movies
        [HttpGet]
        public ActionResult<IEnumerable<Movie>> GetMovies()
        {
            var movies = _movieRepository.GetAllMovies(User);
            return Ok(movies);
        }

        // GET: api/Movies/5
        [HttpGet("{id}")]
        public ActionResult<Movie> GetMovie(int id)
        {
            if (!_movieRepository.MovieExists(id))
                return NotFound("Movie with id: '" + id + "' does not exist.");

            Movie movie = _movieRepository.GetMovie(User, id);

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

            Movie movie = _movieRepository.GetMovie(User, id);

            if (movie.Review == null)
            {
                return NotFound();
            }

            return Ok(movie.Review);
        }



        // GET: api/Movies/5
        [HttpGet("{id}/quotes")]
        public ActionResult<IEnumerable<Quote>> GetMovieQuotes(int id)
        {
            if (!_movieRepository.MovieExists(id))
                return NotFound("Movie with id: '" + id + "' does not exist.");

            Movie movie = _movieRepository.GetMovie(User, id);

            if (movie.Quotes == null)
            {
                return NotFound("Quotes for movie " + id + " does not exist");
            }

            IEnumerable<Quote> quotes = movie.Quotes.Select(q => new Quote
            {
                Id = q.Id,
                Title = q.Title,
                Text = q.Text,
                MovieID = movie.Id
            });

            return Ok(quotes);
        }

        // GET: api/Movies/5
        [HttpGet("{id}/quotes/{quoteId}")]
        public ActionResult<Quote> GetMovieQuote(int id, int quoteId)
        {
            if (!_movieRepository.MovieExists(id))
                return NotFound("Movie with id: '" + id + "' does not exist.");

            Movie movie = _movieRepository.GetMovie(User, id);

            if (movie.Quotes == null)
            {
                return NotFound("Quotes for movie " + id + " does not exist");
            }

            if (!_quoteRepository.QuoteExists(quoteId))
                return NotFound("Quote " + quoteId + " does not exist.");

            Quote quote = movie.Quotes
                .Where(q => q.Id == quoteId)
                .Select(q => new Quote
                {
                    Id = q.Id,
                    Title = q.Title,
                    Text = q.Text,
                    MovieID = movie.Id
                })
                .FirstOrDefault();

            return Ok(quote);
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
            User user = await _context.Users.FindAsync(User.Claims.ToList()[0].Value);

            if (user == null)
                return Unauthorized();

            Category category = _categoryRepository.GetCategory(User, model.CategoryID);
            if (category == null)
                return NotFound("Category: " + model.CategoryID + " not found");

            Movie movie = new Movie
            {
                Author = model.Author,
                Title = model.Title,
                Description = model.Description,
                Year = model.Year,
                ImageURL = model.ImageURL,
                CategoryID = category.Id
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
