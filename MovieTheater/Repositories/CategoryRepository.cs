using Microsoft.EntityFrameworkCore;
using MovieTheater.Helpers;
using MovieTheater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        public IEnumerable<Category> GetAllCategories(ClaimsPrincipal user)
        {
            var categories = _context.Categories.AsQueryable();
            if (user != null && !user.IsInRole(UserRoleDefaults.Admin))
                categories = categories.Where(cat => cat.User.UserName.Equals(user.Identity.Name));

            return categories
                .Include(cat => cat.Movies)
                .ThenInclude(mov => mov.Quotes)
                .Select(cat => new Category
                {
                    Id = cat.Id,
                    Title = cat.Title,
                    Description = cat.Description,
                    UserId = cat.UserId,
                    Movies = cat.Movies.Select(mov => new Movie
                    {
                        Id = mov.Id,
                        Author = mov.Author,
                        Title = mov.Title,
                        Description = mov.Description,
                        Year = mov.Year,
                        Rating = mov.Rating,
                        ImageURL = mov.ImageURL,
                        Review = mov.Review,
                        Quotes = mov.Quotes.Select(q => new Quote
                        {
                            Id = q.Id,
                            Title = q.Title,
                            Text = q.Text
                        })
                    })
                });
        }

        public Category GetCategory(ClaimsPrincipal user, int id)
        {
            var categories = _context.Categories.AsQueryable();
            if (user != null && !user.IsInRole(UserRoleDefaults.Admin))
                categories = categories.Where(cat => cat.User.UserName.Equals(user.Identity.Name));

            return categories
                .Where(cat => cat.Id == id)
                .Include(cat => cat.Movies)
                .ThenInclude(mov => mov.Quotes)
                .Select(cat => new Category
                {
                    Id = cat.Id,
                    Title = cat.Title,
                    Description = cat.Description,
                    UserId = cat.UserId,
                    Movies = cat.Movies.Select(mov => new Movie
                    {
                        Id = mov.Id,
                        Author = mov.Author,
                        Title = mov.Title,
                        Description = mov.Description,
                        Year = mov.Year,
                        Rating = mov.Rating,
                        ImageURL = mov.ImageURL,
                        Review = mov.Review,
                        Quotes = mov.Quotes.Select(q => new Quote
                        {
                            Id = q.Id,
                            Title = q.Title,
                            Text = q.Text
                        })
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
