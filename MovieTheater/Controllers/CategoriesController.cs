﻿using System;
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
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.Select(cat => new Category
            {
                Id = cat.Id,
                Title = cat.Title,
                Description = cat.Description,
                Movies = cat.Movies.ToList()
            }).ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            Category category = _context.Categories
                .Where(cat => cat.Id == id)
                .Select(cat => new Category
            {
                Id = cat.Id,
                Title = cat.Title,
                Description = cat.Description,
                Movies = cat.Movies.ToList()
            }).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        // GET: api/Categories/5
        [HttpGet("{id}/movies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetCategoryMovies(int id)
        {
            List<Movie> movies = _context.Categories
                .Where(cat => cat.Id == id)
                .Select(cat => cat.Movies)
                .FirstOrDefault().ToList();

            if (movies == null)
            {
                return NotFound();
            }

            return movies;
        }

        // GET: api/Categories/5
        [HttpGet("{id}/movies/{movieID}")]
        public async Task<ActionResult<Movie>> GetCategoryMovie(int id, int movieID)
        {
            Movie movie = _context.Categories
                .Where(cat => cat.Id == id)
                .Select(cat => cat.Movies)
                .FirstOrDefault().ToList()
                .Where(mov => mov.Id == movieID)
                .FirstOrDefault();

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }



        // GET: api/Movies/5
        [HttpGet("{id}/movies/{movieID}/review")]
        public async Task<ActionResult<Review>> GetMovieReview(int id, int movieID)
        {
            Review review = _context.Categories
                .Where(cat => cat.Id == id)
                .Select(cat => cat.Movies)
                .FirstOrDefault().ToList()
                .Where(mov => mov.Id == movieID)
                .Select(mov => mov.Review)
                .FirstOrDefault();

            if (review == null)
            {
                return NotFound();
            }

            return review;
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
