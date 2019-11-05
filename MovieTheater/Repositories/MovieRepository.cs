using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MovieTheater.Helpers;
using MovieTheater.Models;

namespace MovieTheater.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly MovieContext _context;

        public MovieRepository()
        {
            _context = new MovieContext();
        }

        public IEnumerable<Movie> GetAllMovies(ClaimsPrincipal user)
        {
            var movies = _context.Movies.AsQueryable();
            if (user != null && !user.IsInRole(UserRoleDefaults.Admin))
                movies = movies.Where(mov => mov.Category.User.UserName.Equals(user.Identity.Name));

            return movies
                .Select(mov => new Movie
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
                });
        }

        public Movie GetMovie(ClaimsPrincipal user, int id)
        {
            var movies = _context.Movies.AsQueryable();
            if (user != null && !user.IsInRole(UserRoleDefaults.Admin))
                movies = movies.Where(mov => mov.Category.User.UserName.Equals(user.Identity.Name));

            return movies
                .Where(mov => mov.Id == id)
                .Select(mov => new Movie
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
                .FirstOrDefault();
        }


        public bool MovieExists(int id)
        {
            return _context.Movies.Any(movie => movie.Id == id);
        }
    }
}
