using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTheater;
using MovieTheater.Models;
using MovieTheater.Models.ViewModels;
using MovieTheater.Repositories;

namespace MovieTheater.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CinemasController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly ICinemaRepository _cinemaRepository;

        public CinemasController(ICinemaRepository cinemaRepository)
        {
            _context = new MovieContext();
            _cinemaRepository = cinemaRepository;
        }

        // GET: api/Cinemas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cinema>>> GetCinemas()
        {
            return Ok(await _cinemaRepository.GetAllCinemas(User));
        }

        // GET: api/Cinemas/5
        [HttpGet("{id}")]
        public ActionResult<Cinema> GetCinema(int id)
        {
            var cinema = _cinemaRepository.GetCinema(User, id);

            if (cinema == null)
            {
                return NotFound();
            }

            return cinema;
        }

        // PUT: api/Cinemas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCinema(int id, Cinema cinema)
        {
            User user = await _context.Users.FindAsync(User.Claims.ToList()[0].Value);

            if (user == null)
                return Unauthorized();

            Cinema dbCinema = _cinemaRepository.GetCinema(User, id);
            if (dbCinema == null)
                return NotFound("Cinema: " + id + " not found");

            if (id != cinema.Id)
            {
                return BadRequest();
            }

            _context.Entry(cinema).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_cinemaRepository.CinemaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cinemas
        [HttpPost]
        public async Task<ActionResult<Cinema>> PostCinema(CreateCinemaViewModel model)
        {
            User user = await _context.Users.FindAsync(User.Claims.ToList()[0].Value);

            if (user == null)
                return Unauthorized();

            Cinema cinema = new Cinema
            {
                Name = model.Name,
                Address = model.Address,
                Capacity = model.Capacity,
                User = user
            };
            _context.Cinemas.Add(cinema);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCinema", new { id = cinema.Id }, new { cinema.Id, cinema.Name, cinema.Address, cinema.Capacity });
        }

        // DELETE: api/Cinemas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cinema>> DeleteCinema(int id)
        {
            User user = await _context.Users.FindAsync(User.Claims.ToList()[0].Value);

            if (user == null)
                return Unauthorized();

            Cinema cinema = _cinemaRepository.GetCinema(User, id);
            if (cinema == null)
                return NotFound("Cinema: " + id + " not found");

            _context.Cinemas.Remove(cinema);
            await _context.SaveChangesAsync();

            return cinema;
        }
    }
}
