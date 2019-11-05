using Microsoft.EntityFrameworkCore;
using MovieTheater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MovieContext _context;

        public CategoryRepository()
        {
            _context = new MovieContext();
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _context.Categories
                .Include(cat => cat.Movies)
                .Select(cat => new Category
                {
                    Id = cat.Id,
                    Title = cat.Title,
                    Description = cat.Description,
                    Movies = cat.Movies.Select(mov => new Movie
                    {
                        Id = mov.Id,
                        Author = mov.Author,
                        Title = mov.Title,
                        Description = mov.Description,
                        Year = mov.Year,
                        Rating = mov.Rating,
                        ImageURL = mov.ImageURL,
                        Review = mov.Review
                    })
                });
        }

        public Category GetCategory(int id)
        {
            return _context.Categories
                .Where(cat => cat.Id == id)
                .Include(cat => cat.Movies)
                .Select(cat => new Category
                {
                    Id = cat.Id,
                    Title = cat.Title,
                    Description = cat.Description,
                    Movies = cat.Movies.Select(mov => new Movie
                    {
                        Id = mov.Id,
                        Author = mov.Author,
                        Title = mov.Title,
                        Description = mov.Description,
                        Year = mov.Year,
                        Rating = mov.Rating,
                        ImageURL = mov.ImageURL,
                        Review = mov.Review
                    })
                })
                .FirstOrDefault();
        }

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(category => category.Id == id);
        }
    }
}
