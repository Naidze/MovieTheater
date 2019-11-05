using MovieTheater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Repositories
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAllMovies();

        Movie GetMovie(int id);

        bool MovieExists(int id);
    }
}
