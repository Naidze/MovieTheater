using MovieTheater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieTheater.Repositories
{
    public interface ICinemaRepository
    {
        Task<IEnumerable<Cinema>> GetAllCinemas(ClaimsPrincipal user);

        Cinema GetCinema(ClaimsPrincipal user, int id);

        bool CinemaExists(int id);
    }
}
