using MovieTheater.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieTheater.Repositories
{
    public interface ICinemaRepository
    {
        Task<IEnumerable<Cinema>> GetAllCinemas();

        Task<Cinema> GetCinema(int id);

        bool CinemaExists(int id);
    }
}
