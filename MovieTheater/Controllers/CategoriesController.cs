using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Helpers;
using MovieTheater.Models;
using MovieTheater.Models.ViewModels;
using MovieTheater.Repositories;
using MovieTheater.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _context = new MovieContext();
            _categoryRepository = categoryRepository;
        }

        // GET: api/Categories
        [HttpGet]
        public ActionResult<IEnumerable<Category>> GetCategories()
        {
            var categories = _categoryRepository.GetAllCategories(User);
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public ActionResult<Category> GetCategory(int id)
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound("Category with id: '" + id + "' does not exist.");

            var category = _categoryRepository.GetCategory(User, id);

            return Ok(category);
        }

        // GET: api/Categories/5
        [HttpGet("{id}/movies")]
        public ActionResult<IEnumerable<Movie>> GetCategoryMovies(int id)
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound("Category with id: '" + id + "' does not exist.");

            var category = _categoryRepository.GetCategory(User, id);

            if (category.Movies == null)
            {
                return NotFound("Movies for category: '" + id + "' was not found.");
            }

            return Ok(category.Movies);
        }

        // GET: api/Categories/5
        [HttpGet("{id}/movies/{movieID}")]
        public ActionResult<Movie> GetCategoryMovie(int id, int movieID)
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound("Category with id: '" + id + "' does not exist.");

            Category category = _categoryRepository.GetCategory(User, id);

            Movie movie = category.Movies
                .Where(mov => mov.Id == movieID)
                .FirstOrDefault();

            if (movie == null)
            {
                return NotFound("Movie with id: '" + movieID + "' in category with id: '" + id + "' was not found");
            }

            return Ok(movie);
        }



        // GET: api/Categories/5/movies/6/review
        [HttpGet("{id}/movies/{movieID}/review")]
        public ActionResult<Review> GetMovieReview(int id, int movieID)
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound("Category with id: '" + id + "' does not exist.");

            Category category = _categoryRepository.GetCategory(User, id);

            Movie movie = category.Movies
                .Where(mov => mov.Id == movieID)
                .FirstOrDefault();

            if (movie.Review == null)
            {
                return NotFound("Review for movie with id: '" + movieID + "' in category with id: " + id + " was not found");
            }

            return Ok(movie.Review);
        }





        // GET: api/Categories/5/movies/6/review
        [HttpGet("{id}/movies/{movieID}/quotes")]
        public ActionResult<IEnumerable<Quote>> GetMovieQuotes(int id, int movieID)
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound("Category with id: '" + id + "' does not exist.");

            Category category = _categoryRepository.GetCategory(User, id);

            Movie movie = category.Movies
                .Where(mov => mov.Id == movieID)
                .FirstOrDefault();

            if (movie.Quotes == null)
            {
                return NotFound("Quotes for movie with id: '" + movieID + "' in category with id: " + id + " was not found");
            }

            IEnumerable<Quote> quotes = movie.Quotes.Select(q => new Quote
            {
                Id = q.Id,
                Title = q.Title,
                Text = q.Text
            });

            return Ok(quotes);
        }

        // GET: api/Categories/5/movies/6/review
        [HttpGet("{id}/movies/{movieID}/quotes/{quoteID}")]
        public ActionResult<Quote> GetMovieQuotes(int id, int movieID, int quoteID)
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound("Category with id: '" + id + "' does not exist.");

            Category category = _categoryRepository.GetCategory(User, id);

            Movie movie = category.Movies
                .Where(mov => mov.Id == movieID)
                .FirstOrDefault();

            if (movie.Quotes == null)
            {
                return NotFound("Quotes for movie with id: '" + movieID + "' in category with id: " + id + " was not found");
            }

            Quote quote = movie.Quotes
                .Where(q => q.Id == quoteID)
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


        // PUT: api/Categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_categoryRepository.CategoryExists(id))
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

        // POST: api/Categories
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(CreateCategoryViewModel model)
        {
            User user = await _context.Users.FindAsync(User.Claims.ToList()[0].Value);

            if (user == null)
                return Unauthorized();

            Category category = new Category
            {
                Title = model.Title,
                Description = model.Description,
                Movies = model.Movies,
                User = user
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, new { category.Id, category.Title, category.Description });
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Category>> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new { category.Id, category.Title, category.Description });
        }
    }
}
