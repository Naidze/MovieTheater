using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheater;
using MovieTheater.Models;
using MovieTheater.Models.ViewModels;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly MovieContext _context;

        public CategoriesController(MovieContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public ActionResult<IEnumerable<object>> GetCategories()
        {
            var categories = _context.Categories
                .Include(cat => cat.Movies)
                .Select(cat => new
                {
                    cat.Id,
                    cat.Title,
                    cat.Description,
                    Movies = cat.Movies
                        .Select(mov => new
                        {
                            mov.Id,
                            mov.Author,
                            mov.Title,
                            mov.Description,
                            mov.Year,
                            mov.Review
                        })
                });
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            var category = _context.Categories
                .Where(cat => cat.Id == id)
                .Select(cat => new
                {
                    cat.Id,
                    cat.Title,
                    cat.Description,
                    Movies = cat.Movies
                        .Select(mov => new
                        {
                            mov.Id,
                            mov.Author,
                            mov.Title,
                            mov.Description,
                            mov.Year,
                            mov.Review
                        })
                }).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        // GET: api/Categories/5
        [HttpGet("{id}/movies")]
        public ActionResult<IEnumerable<object>> GetCategoryMovies(int id)
        {
            var movies = _context.Categories
                .Where(cat => cat.Id == id)
                .Select(cat => cat.Movies.Select(mov => new
                {
                    mov.Id,
                    mov.Author,
                    mov.Title,
                    mov.Description,
                    mov.Year,
                    mov.Review
                }))
                .FirstOrDefault();

            if (movies == null)
            {
                return NotFound();
            }

            return Ok(movies);
        }

        // GET: api/Categories/5
        [HttpGet("{id}/movies/{movieID}")]
        public ActionResult<object> GetCategoryMovie(int id, int movieID)
        {
            var movie = _context.Categories
                .Where(cat => cat.Id == id)
                .Select(cat => cat.Movies
                    .Select(mov => new
                    {
                        mov.Id,
                        mov.Author,
                        mov.Title,
                        mov.Description,
                        mov.Year,
                        mov.Review
                    })
                )
                .FirstOrDefault()
                .Where(mov => mov.Id == movieID)
                .FirstOrDefault();

            if (movie == null)
            {
                return NotFound();
            }

            return Ok(movie);
        }



        // GET: api/Movies/5
        [HttpGet("{id}/movies/{movieID}/review")]
        public ActionResult<object> GetMovieReview(int id, int movieID)
        {
            var review = _context.Categories
                .Where(cat => cat.Id == id)
                .Select(cat => cat.Movies
                    .Select(mov => new
                    {
                        mov.Id,
                        mov.Review
                    })
                )
                .FirstOrDefault()
                .Where(mov => mov.Id == movieID)
                .FirstOrDefault()
                .Review;

            if (review == null)
            {
                return NotFound();
            }

            return Ok(review);
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
                if (!CategoryExists(id))
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
            Category category = new Category
            {
                Title = model.Title,
                Description = model.Description,
                Movies = model.Movies
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
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

            return category;
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
