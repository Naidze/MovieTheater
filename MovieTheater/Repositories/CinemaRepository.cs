using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieTheater.Helpers;
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

        public async Task<IEnumerable<Cinema>> GetAllCinemas(ClaimsPrincipal user)
        {
            var cinemas = _context.Cinemas.AsQueryable();
            if (user != null && !user.IsInRole(UserRoleDefaults.Admin))
                cinemas = cinemas.Where(cat => cat.User.UserName.Equals(user.Identity.Name));

            return await cinemas
                .Select(cinema => new Cinema
                {
                    Id = cinema.Id,
                    Name = cinema.Name,
                    Address = cinema.Address,
                    Capacity = cinema.Capacity,
                    UserId = cinema.UserId
                })
                .ToListAsync();
        }

        public Cinema GetCinema(ClaimsPrincipal user, int id)
        {
            var cinemas = _context.Cinemas.AsQueryable();
            if (user != null && !user.IsInRole(UserRoleDefaults.Admin))
                cinemas = cinemas.Where(cat => cat.User.UserName.Equals(user.Identity.Name));

            return cinemas
                .Where(cinema => cinema.Id.Equals(id))
                .Select(cinema => new Cinema
                {
                    Id = cinema.Id,
                    Name = cinema.Name,
                    Address = cinema.Address,
                    Capacity = cinema.Capacity,
                    UserId = cinema.UserId
                })
                .FirstOrDefault();
        }

        public bool CinemaExists(int id)
        {
            return _context.Cinemas
                .Any(cinema => cinema.Id == id);
        }
    }
}
