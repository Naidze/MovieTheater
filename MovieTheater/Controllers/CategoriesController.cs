﻿using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = UserRoleDefaults.Admin)]
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
        public ActionResult<IEnumerable<object>> GetCategories()
        {
            var categories = _categoryRepository.GetAllCategories();
            return Ok(categories);
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public IActionResult GetCategory(int id)
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound("Category with id: " + id + "does not exist.");

            var category = _categoryRepository.GetCategory(id);

            return Ok(category);
        }

        // GET: api/Categories/5
        [HttpGet("{id}/movies")]
        public ActionResult<IEnumerable<Movie>> GetCategoryMovies(int id)
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound("Category with id: " + id + "does not exist.");

            var category = _categoryRepository.GetCategory(id);

            if (category.Movies == null)
            {
                return NotFound("Movies for category: " + id + " was not found.");
            }

            return Ok(category.Movies);
        }

        // GET: api/Categories/5
        [HttpGet("{id}/movies/{movieID}")]
        public ActionResult<Movie> GetCategoryMovie(int id, int movieID)
        {
            if (!_categoryRepository.CategoryExists(id))
                return NotFound("Category with id: " + id + "does not exist.");

            Category category = _categoryRepository.GetCategory(id);

            Movie movie = category.Movies
                .Where(mov => mov.Id == movieID)
                .FirstOrDefault();

            if (movie == null)
            {
                return NotFound("Movie with id: " + movieID + " in category with id: " + id + " was not found");
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
                return NotFound("Review for movie with id: " + movieID + " in category with id: " + id + " was not found");
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
