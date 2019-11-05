using MovieTheater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieTheater.Repositories
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAllMovies(ClaimsPrincipal user);

        Movie GetMovie(ClaimsPrincipal user, int id);

        bool MovieExists(int id);
    }
}
