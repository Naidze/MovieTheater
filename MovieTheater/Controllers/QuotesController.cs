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
    public class QuotesController : ControllerBase
    {
        private readonly MovieContext _context;
        private readonly IQuoteRepository _quoteRepository;
        private readonly IMovieRepository _movieRepository;

        public QuotesController(IQuoteRepository quoteRepository, IMovieRepository movieRepository)
        {
            _context = new MovieContext();
            _movieRepository = movieRepository;
            _quoteRepository = quoteRepository;
        }

        // GET: api/Quotes
        [HttpGet]
        public ActionResult<IEnumerable<Quote>> GetQuotes()
        {
            return Ok(_quoteRepository.GetAllQuotes(User));
        }

        // GET: api/Quotes/5
        [HttpGet("{id}")]
        public ActionResult<Quote> GetQuote(int id)
        {
            if (!_quoteRepository.QuoteExists(id))
                return NotFound("Quote " + id + " does not exist.");

            var quote = _quoteRepository.GetQuote(User, id);

            if (quote == null)
            {
                return NotFound();
            }

            return Ok(quote);
        }

        // PUT: api/Quotes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuote(int id, Quote quote)
        {
            User user = await _context.Users.FindAsync(User.Claims.ToList()[0].Value);

            if (user == null)
                return Unauthorized();

            Quote dbQuote = _quoteRepository.GetQuote(User, id);
            if (dbQuote == null)
                return NotFound("Quote: " + id + " not found");

            if (id != quote.Id)
            {
                return BadRequest();
            }

            _context.Entry(quote).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_quoteRepository.QuoteExists(id))
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

        // POST: api/Quotes
        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(CreateQuoteViewModel model)
        {
            User user = await _context.Users.FindAsync(User.Claims.ToList()[0].Value);

            if (user == null)
                return Unauthorized();

            Movie movie = _movieRepository.GetMovie(User, model.MovieID);
            if (movie == null)
                return NotFound("Movie " + model.MovieID + " not found.");

            Quote quote = new Quote
            {
                Title = model.Title,
                Text = model.Text,
                MovieID = movie.Id
            };
            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQuote", new { id = quote.Id }, quote);
        }

        // DELETE: api/Quotes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Quote>> DeleteQuote(int id)
        {
            User user = await _context.Users.FindAsync(User.Claims.ToList()[0].Value);

            if (user == null)
                return Unauthorized();

            Quote quote = _quoteRepository.GetQuote(User, id);
            if (quote == null)
                return NotFound("Quote: " + id + " not found");

            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();

            return quote;
        }
    }
}
