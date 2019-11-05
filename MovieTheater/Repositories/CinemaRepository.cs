using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Models;

namespace MovieTheater.Repositories
{
    public class CinemaRepository : ICinemaRepository
    {
        private readonly MovieContext _context;

        public CinemaRepository()
        {
            _context = new MovieContext();
        }

        public async Task<IEnumerable<Cinema>> GetAllCinemas()
        {
            return await _context.Cinemas
                .Select(cinema => new Cinema
                {
                    Id = cinema.Id,
                    Name = cinema.Name,
                    Address = cinema.Address,
                    Capacity = cinema.Capacity
                })
                .ToListAsync();
        }

        public async Task<Cinema> GetCinema(int id)
        {
            return await _context.Cinemas.FindAsync(id);
        }

        public bool CinemaExists(int id)
        {
            return _context.Cinemas
                .Any(cinema => cinema.Id == id);
        }
    }
}
