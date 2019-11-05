using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public IEnumerable<Movie> GetAllMovies()
        {
            return _context.Movies
                .Select(mov => new Movie
                {
                    Id = mov.Id,
                    Author = mov.Author,
                    Title = mov.Title,
                    Description = mov.Description,
                    Year = mov.Year,
                    Rating = mov.Rating,
                    ImageURL = mov.ImageURL,
                    Review = mov.Review
                });
        }

        public Movie GetMovie(int id)
        {
            return _context.Movies
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
                    Review = mov.Review
                })
                .FirstOrDefault();
        }


        public bool MovieExists(int id)
        {
            return _context.Movies.Any(movie => movie.Id == id);
        }
    }
}
